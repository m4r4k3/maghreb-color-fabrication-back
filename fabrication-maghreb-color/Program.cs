using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.application.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using fabrication_maghreb_color.Config.Sage;
using Serilog;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

//// Configure Serilog for logging with daily rolling file
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

// Use Serilog for logging
builder.Host.UseSerilog();

// Set the server to listen on all network interfaces
builder.WebHost.UseUrls("http://0.0.0.0:5254");

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
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProjetService>();
builder.Services.AddScoped<ChargeCompteService>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<CompteService>();
builder.Services.AddScoped<MatiereService>();
builder.Services.AddScoped<PreparationFabricationService>();
builder.Services.AddScoped<MachineService>();
builder.Services.AddSingleton<SageOM>();

// Add Swagger for API documentation
builder.Services.AddSwaggerGen();

// Configure JSON serialization options
builder.Services.AddControllers()
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

// Configure CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>

{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        // Allow requests from specific frontend URLs
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();

        policy.WithOrigins("http://192.168.1.247:5173")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
    });
});

// Add authorization services
builder.Services.AddAuthorization();

// Add controllers

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Build the application
var app = builder.Build();

// Configure Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = string.Empty; // Make Swagger UI accessible at root
});

// Redirect HTTP to HTTPS (commented out configuration for development)
// Uncomment in production
app.UseHttpsRedirection();

// Serve static files
app.UseStaticFiles();

// Apply CORS policy
app.UseCors("AllowSpecificOrigin");

// Create a scope to resolve SageOM service
using (var scope = app.Services.CreateScope())
{
    var sageOM = scope.ServiceProvider.GetRequiredService<SageOM>();
}

// Configure routing
app.UseRouting();

// Note: Authentication and Authorization are currently commented out
// app.UseAuthentication();
// app.UseAuthorization();

// Map controllers
app.MapControllers();

// Default route returns Not Found
app.MapGet("/", () => Results.NotFound());

// Run the application
app.Run();