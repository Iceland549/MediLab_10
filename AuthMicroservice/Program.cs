// Import dependencies for service injection, security, database, and authentication.
using AuthMicroservice.Application.Interfaces;
using AuthMicroservice.Application.Services;
using AuthMicroservice.Domain.Models;
using AuthMicroservice.Infrastructure.Data;
using AuthMicroservice.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Listen on port 5001 for this microservice.
builder.WebHost.UseUrls("http://+:5001");

// Add a health check endpoint for monitoring.
builder.Services.AddHealthChecks();

// Configure SQL Server database with retry on failure for resilience.
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

// Add Identity for user and role management, using EF Core for storage.
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Register repositories and business services for authentication.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure CORS to allow the React app to communicate with the API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add support for API controllers.
builder.Services.AddControllers();

// Retrieve JWT parameters from configuration.
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException
        ("JWT secret key (Jwt:Key) is not configured in appsettings.json.");

// Configure JWT authentication to secure API endpoints.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        // Token validation parameters: signature, audience, issuer, and lifetime.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add authorization services.
builder.Services.AddAuthorization();

// Add Swagger/OpenAPI documentation for the API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable the configured CORS policy for React.
app.UseCors("AllowReactApp");

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Automatically apply database migrations and seed data with retry logic.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var retry = 0;
    var maxRetries = 10;
    var delay = TimeSpan.FromSeconds(5);

    while (retry < maxRetries)
    {
        try
        {
            var context = services.GetRequiredService<AuthDbContext>();
            await context.Database.MigrateAsync(); // Apply EF migrations.
            await AuthDbInitializer.SeedAsync(services); // Seed initial users/data.
            break;
        }
        catch (Exception ex)
        {
            retry++;
            Console.WriteLine($"Attempt {retry}/{maxRetries} failed: {ex.Message}");
            await Task.Delay(delay); // Wait before retrying 
        }
    }
    if (retry == maxRetries)
    {
        Console.WriteLine("Failed to seed the database after multiple retries.");
    }
}

// Add a health check endpoint for monitoring.
app.MapHealthChecks("/health");

app.Run();
