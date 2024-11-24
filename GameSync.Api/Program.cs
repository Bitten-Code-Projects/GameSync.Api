namespace GameSync.Api;

using System.Reflection;
using FluentValidation;
using GameSync.Api.Shared.Middleware;
using GameSync.Application.Examples.Interfaces;
using GameSync.Domain.GameSync.Interfaces;
using GameSync.Infrastructure.GameSync;
using Microsoft.OpenApi.Models;

/// <summary>
/// Main Program class.
/// </summary>
public class Program
{
    /// <summary>
    /// Main method.
    /// </summary>
    /// <param name="args">Collection of start arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "GameSync", Version = "v1" });

            var filePath = Path.Combine(AppContext.BaseDirectory, "GameSync.Api.xml");
            c.IncludeXmlComments(filePath);
        });

        var applicationAssembly = Assembly.Load("GameSync.Application");
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        builder.Services.AddDbContext<GameSyncDbContext>();
        builder.Services.AddAutoMapper(applicationAssembly);

        builder.Services.AddScoped<IGameSyncRepository, GameSyncRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.ConfigureExceptionHandler();

        app.MapControllers();

        app.Run();
    }
}
