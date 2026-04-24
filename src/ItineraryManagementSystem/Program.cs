using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using ItineraryManagementSystem.Interfaces;
using ItineraryManagementSystem.Models;
using ItineraryManagementSystem.Services;
using ItineraryManagementSystem.Validators;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace ItineraryManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.Load();

            var connectionString = Environment.GetEnvironmentVariable("DB_SQL_CONNECTION");

            // Serilog configuration
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(path: $"Logs/{DateTime.Now:yyyy-MM-dd}/log-.txt", rollingInterval: RollingInterval.Hour)
                .CreateLogger();

            builder.Host.UseSerilog();

            // Add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateItineraryValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ItineraryQueryParamsValidator>();
            builder.Services.AddScoped<IItineraryRepository, ItineraryRepository>();
            builder.Services.AddScoped<IItineraryService, ItineraryService>();
            builder.Services.AddDbContext<ItineraryDbContext>(options =>
                options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString)));

            var app = builder.Build();

            app.UseSerilogRequestLogging(); // logs every HTTP request

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Itinerary Api v1");
                    options.RoutePrefix = "itinerary";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}