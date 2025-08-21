using Microsoft.EntityFrameworkCore;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Application.Middleware;
using WeatherForecast.Application.Services;
using WeatherForecast.Domain.Options;
using WeatherForecast.Infrastructure;
using WeatherForecast.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddScoped<IWeatherForecastRepository, ForecastRepository>();
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("WeatherApi"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();
    



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();



app.Run();
