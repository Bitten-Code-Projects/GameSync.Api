namespace GameSync.Api;

using System.Reflection;
using FluentValidation;
using GameSync.Api.Middleware;
using GameSync.Api.Shared.Middleware;
using GameSync.Application.EmailInfrastructure;
using GameSync.Infrastructure.Context;
using GameSync.Infrastructure.Context.Models;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

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

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(x => x.AddOtlpExporter(y =>
        {
            x.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                .AddService("GameSync.Api")
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector()
                .AddAttributes(new Dictionary<string, object>
                {
                    ["host.type"] = Environment.MachineName,
                    ["deployment.environment"] = builder.Environment.EnvironmentName,
                }));

            x.IncludeScopes = true;
            x.IncludeFormattedMessage = true;

            y.Endpoint = new Uri(Environment.GetEnvironmentVariable("SEQ_API_URL")!);
            y.Protocol = OtlpExportProtocol.HttpProtobuf;
            y.Headers = $"X-Seq-ApiKey={Environment.GetEnvironmentVariable("SEQ_API_KEY")}";
        }));

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddScoped<ISmtpClient, SmtpClientWrapper>();
        builder.Services.AddScoped<IEmailMessageFactory, EmailMessageFactory>();
        builder.Services.AddScoped<IEmailService, EmailService>();

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
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddAutoMapper(applicationAssembly);

        builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<AppDbContext>();
        builder.Services.AddAuthorization();

        // Load environment variables
        configuration["EmailSettings:Password"] = Environment.GetEnvironmentVariable("BCP_GS_EMAIL_PASS");
        configuration["EmailSettings:AuthLogin"] = Environment.GetEnvironmentVariable("BCP_GS_EMAIL_USER");
        configuration["EmailSettings:SenderEmail"] = Environment.GetEnvironmentVariable("BCP_GS_SENDER");
        builder.Services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        builder.Services.AddSingleton<IEmailService, EmailService>();

        var app = builder.Build();

        app.MapGroup("/account").MapIdentityApi<ApplicationUser>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        app.ConfigureExceptionHandler(logger);

        app.UseRequestBodyLogging();
        app.UseResponseBodyMiddleware();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
