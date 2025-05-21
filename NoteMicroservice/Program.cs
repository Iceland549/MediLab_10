using Microsoft.Extensions.Options;
using NoteMicroservice.Application.Interfaces;
using NoteMicroservice.Application.Services;
using NoteMicroservice.Infrastructure.Data;
using NoteMicroservice.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(
    builder.Configuration.GetSection("MongoDbConfig"));

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5004";
        options.RequireHttpsMetadata = false;
        options.Audience = "notemicroservice";
    });

builder.Services.AddAuthorization();

builder.Services.AddTransient<NotesSeeder>();


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

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<NotesSeeder>();
    await seeder.SeedAsync();
}

app.Run();
