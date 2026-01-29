using Microsoft.EntityFrameworkCore;
using Lab_3_Backend.Data;
using Lab_3_Backend.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);


//DbContext
builder.Services.AddDbContext<CarDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health Checks для мониторинга состояния
builder.Services.AddHealthChecks().AddDbContextCheck<CarDbContext>();

//ORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()       // Разрешить запросы с любого домена
              .AllowAnyMethod()       // Разрешить любые HTTP методы (GET, POST, PUT, DELETE)
              .AllowAnyHeader();      // Разрешить любые заголовки
    });
});

var app = builder.Build();


// Автоматическое создание БД при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CarDbContext>();
    dbContext.Database.EnsureCreated();
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware в правильном порядке
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

// Endpoints
app.MapControllers();

app.MapHealthChecks("/health");

app.Run();