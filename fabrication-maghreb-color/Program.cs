using System.Net;
using System.Text;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddDbContext<MainContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PROGRAM_DB")));

builder.Services.AddDbContext<SageContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SAGE_DB")));

configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var key = Encoding.ASCII.GetBytes(configuration["AppSettings:JWT_KEY"]);
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProduitService>();
builder.Services.AddScoped<ProjetService>();
builder.Services.AddScoped<ArticleService>();

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
       options.Events = new JwtBearerEvents
       {
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
           OnMessageReceived = context =>
       {
           var token = context.Request.Cookies["auth"];
           context.Token = token;
           return Task.CompletedTask;
       }
       };

   });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); // Add this line

var app = builder.Build();


// Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.NotFound());

app.Run();
