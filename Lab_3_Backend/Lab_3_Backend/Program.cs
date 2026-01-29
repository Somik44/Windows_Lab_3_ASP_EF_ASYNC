using Microsoft.EntityFrameworkCore;
using Lab_3_Backend.Data;
using Lab_3_Backend.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);


//DBCONTEXT
builder.Services.AddDbContext<CarDbContext>(options =>
{
    string connectionString;

    if (builder.Environment.EnvironmentName == "Primary")
    {
        connectionString = builder.Configuration.GetConnectionString("PrimaryConnection");
    }
    else if (builder.Environment.EnvironmentName == "Secondary")
    {
        connectionString = builder.Configuration.GetConnectionString("SecondaryConnection");
    }
    else
    {
        connectionString = builder.Configuration.GetConnectionString("PrimaryConnection");
    }

    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<CarDbContext>()
    .AddCheck("InstanceHealth", () =>
    {
        var env = builder.Environment.EnvironmentName;
        return HealthCheckResult.Healthy($"Instance: {env}");
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

//Создание БД при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CarDbContext>();
    try
    {
        var created = dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка создания базы данных: {ex.Message}");
    }
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Car API v1");
    options.RoutePrefix = "swagger";
});

// Middleware
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

// Endpoints
app.MapControllers();

// Health Check
if (builder.Environment.EnvironmentName == "Primary")
{
    app.MapHealthChecks("/health-primary");
}
else if (builder.Environment.EnvironmentName == "Secondary")
{
    app.MapHealthChecks("/health-secondary");
}
else
{
    app.MapHealthChecks("/health");
}
app.Run();