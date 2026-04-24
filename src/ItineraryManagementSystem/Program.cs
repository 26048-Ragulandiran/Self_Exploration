using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using ItineraryManagementSystem.Grpc;
using ItineraryManagementSystem.Interfaces;
using ItineraryManagementSystem.Models;
using ItineraryManagementSystem.Services;
using ItineraryManagementSystem.Validators;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace ItineraryManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.Load();

            var connectionString =
                Environment.GetEnvironmentVariable("DB_SQL_CONNECTION");

            /* ---------------- KESTREL CONFIG (MOVE HERE) ---------------- */

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(7118, obj =>
                {
                    obj.Protocols = HttpProtocols.Http2;
                    obj.UseHttps();
                });
            });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: $"Logs/{DateTime.Now:yyyy-MM-dd}/log-.txt",
                    rollingInterval: RollingInterval.Hour,
                    retainedFileCountLimit: 7,
                    shared: true)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateItineraryValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ItineraryQueryParamsValidator>();

            builder.Services.AddScoped<IItineraryRepository, ItineraryRepository>();
            builder.Services.AddScoped<IItineraryService, ItineraryService>();

            builder.Services.AddDbContext<ItineraryDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddGrpc();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Itinerary API v1");
                    options.RoutePrefix = "itinerary";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // For RestApi
            app.MapControllers();

            // For gRPC calls
            app.MapGrpcService<ItineraryGrpcService>();

            app.Run();
        }
    }
}