using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using fabrication_maghreb_color.Config.Sage;
using Serilog;
using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.application.repository;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authorization;
using fabrication_maghreb_color.application.Authorization;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc.Authorization;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;



//// Configure Serilog for logging with daily rolling file
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

// Use Serilog for logging
builder.Host.UseSerilog();

// Set the server to listen on all network interfaces
builder.WebHost.UseUrls("http://0.0.0.0:5253", "http://0.0.0.0:5254");

// Get the configuration
var configuration = builder.Configuration;

// Configure database contexts
// Main application database context
builder.Services.AddDbContext<MainContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PROGRAM_DB")));

// Sage database context
builder.Services.AddDbContext<SageContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SAGE_DB")));

// Load configuration from appsettings.json
configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Prepare JWT authentication key
var key = Encoding.ASCII.GetBytes(configuration["AppSettings:JWT_KEY"]);

// Register application services
builder.Services.AddScoped<IProjetRepository, ProjetRepository>();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMatiereRepository, MatiereRepository>();
builder.Services.AddScoped<ICompteRepository, CompteRepository>();
builder.Services.AddScoped<IChargeCompteRepository, ChargeCompteRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IFabricationRepository, FabricationRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IDetailsRepository, DetailsRepository>();


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProjetService>();
builder.Services.AddScoped<ChargeCompteService>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<CompteService>();
builder.Services.AddScoped<MatiereService>();
builder.Services.AddScoped<PreparationFabricationService>();
builder.Services.AddScoped<MachineService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>(); // or your implementation
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<PermissionRepository>();
builder.Services.AddScoped<IDetailsService, DetailsService>();

builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<SageOM>();


// Add Swagger for API documentation
builder.Services.AddSwaggerGen();

// Configure JSON serialization options
builder.Services.AddControllers(options =>
{
    // Global authorization policy
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = false,
        ValidateAudience = false,
           ValidateLifetime = true,
           IssuerSigningKey = new SymmetricSecurityKey(key)
       };

       // Custom JWT Bearer events
       options.Events = new JwtBearerEvents
       {
           // Custom unauthorized response
           OnChallenge = async context =>
           {
               if (!context.Response.HasStarted)
               {
                   context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                   context.Response.ContentType = "application/json";
                   await context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
               }
               context.HandleResponse();
           },
           // Read token from authentication cookie
           OnMessageReceived = context =>
           {
               var token = context.Request.Cookies["auth"];
               context.Token = token;
               return Task.CompletedTask;
           }
       };
   });

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MainContext>();
    var permissions = context.PoliciesDbo.Select(e => e.Name).ToList();

    builder.Services.AddAuthorization(options =>
    {
        foreach (var permission in permissions)
        {
            options.AddPolicy(permission, policy =>
                policy.Requirements.Add(new PermissionRequirement(permission)));
        }
    });
}


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true) // Allow all origins
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Allow credentials
    });
});



// Add this BEFORE app.UseAuthorization();

// Add authorization services
builder.Services.AddAuthorization();

// Add controllers

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Build the application
var app = builder.Build();

app.Use(async (context, next) =>
{
    // Add security headers
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

    await next();
});
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = new List<CultureInfo> { cultureInfo },
    SupportedUICultures = new List<CultureInfo> { cultureInfo }
});

// Configure Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = string.Empty; // Make Swagger UI accessible at root
});

// Redirect HTTP to HTTPS (commented out configuration for development)
// Uncomment in production

// Serve static files
app.UseStaticFiles();

// Create a scope to resolve SageOM service
using (var scope = app.Services.CreateScope())
{
    var sageOM = scope.ServiceProvider.GetRequiredService<SageOM>();
}

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Run the application
app.Run();