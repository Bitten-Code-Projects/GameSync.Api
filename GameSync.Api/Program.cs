namespace GameSync.Api;

using System.Reflection;
using FluentValidation;
using GameSync.Api.Shared.Middleware;
using GameSync.Application.Examples.Interfaces;
using GameSync.Infrastructure.Examples;
using MediatR;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var applicationAssembly = Assembly.Load("GameSync.Application");
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        builder.Services.AddDbContext<ExampleDbContext>();
        builder.Services.AddAutoMapper(applicationAssembly);

        builder.Services.AddScoped<IExampleRepository, ExampleRepository>();

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
