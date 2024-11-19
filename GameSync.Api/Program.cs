namespace GameSync.Api;

using System.Reflection;
using FluentValidation;
using GameSync.Api.Shared.Middleware;
using GameSync.Application.Examples.Interfaces;
using GameSync.Infrastructure.Context;
using GameSync.Infrastructure.Context.Models;
using GameSync.Infrastructure.Examples;
using Microsoft.AspNetCore.Identity;
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

        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "GameSync", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
            });

            var filePath = Path.Combine(AppContext.BaseDirectory, "GameSync.Api.xml");
            opt.IncludeXmlComments(filePath);

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        });
        });

        var applicationAssembly = Assembly.Load("GameSync.Application");
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        builder.Services.AddDbContext<ExampleDbContext>();
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddAutoMapper(applicationAssembly);

        builder.Services.AddIdentityApiEndpoints<User>((options) =>
        {
            options.User.RequireUniqueEmail = false;
            options.SignIn.RequireConfirmedEmail = false;
        }).AddEntityFrameworkStores<AppDbContext>();
        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IExampleRepository, ExampleRepository>();

        var app = builder.Build();

        app.MapGroup("/account").MapIdentityApi<User>();

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
