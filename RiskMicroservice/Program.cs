using RiskMicroservice.Application.Interfaces;
using RiskMicroservice.Application.Services;
using RiskMicroservice.Infrastructure.Clients;
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IRiskService,RiskService>();
builder.Services.AddScoped<IPatientClient, PatientClient>();
builder.Services.AddScoped<INoteClient, NoteClient>();

builder.Services.AddControllers();

// Authentication JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5004";
        options.RequireHttpsMetadata = false;
        options.Audience = "riskmicroservice";
    });
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
