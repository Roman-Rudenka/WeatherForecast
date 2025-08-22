using Microsoft.EntityFrameworkCore;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Application.Middleware;
using WeatherForecast.Application.Services;
using WeatherForecast.Domain.Options;
using WeatherForecast.Infrastructure;
using WeatherForecast.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>().Value;
    options.UseNpgsql(dbOptions.DefaultConnection);
});

builder.Services.AddControllers();
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
app.UseMiddleware<ApiExceptionMiddleware>();
app.MapControllers();

app.Run();
