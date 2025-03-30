using EnhancedPensionSystem_Application.Helpers.DTOs.Configs;
using EnhancedPensionSystem_Application.Helpers.DTOs.CustomErrors;
using EnhancedPensionSystem_Application.Helpers.DTOValidations;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.Services.Implementations;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using EnhancedPensionSystem_Application.UnitOfWork.Implementations;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.DataContext;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

namespace EnhancedPensionSystem_WebAPP.Extensions;

public static class ServiceExtensions
{
    public static void
        SetupEmailConfiguration
        (this IServiceCollection services, IConfigurationBuilder configurationBuilder)
    {
        IConfiguration configuration = configurationBuilder
            .AddEnvironmentVariables("PAYBIGISMTP__")
            .Build();
        services.Configure<EmailConfig>(configuration);
    }

    public static void 
        ConfigureSwaggerDocumentations(this IServiceCollection services)
    {
        services.AddSwaggerGen(opts => {
            string basePath = Directory.GetCurrentDirectory();
            string controllersPath = Path.Combine(basePath, "Controllers");
            var xmlFile = "APIDocumentation.xml";
            var xmlPath = Path.Combine(controllersPath, xmlFile);
            var documentationPath = Path.GetFullPath(xmlPath);
            opts.IncludeXmlComments(documentationPath);
        });
    }

    public static void 
        ConfigureUserIdentityManager(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>(o =>
        {
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 8;
            o.Password.RequiredUniqueChars = 0;
            o.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<UserManager<AppUser>>();
    }

    public static void
        ConfigureController
        (this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(opts =>
            opts.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .SelectMany(ms => ms.Value.Errors
                    .Select(e => new ValidationError
                    {
                        FieldName = ms.Key,
                        ErrorMessage = e.ErrorMessage
                    }))
                    .ToList();

                return new BadRequestObjectResult
                (
                    StandardResponse<IEnumerable<ValidationError>>.Failed
                    (data: errors, errorMessage: "One or more validation errors occurred", 400)
                );
            });
    }

    public static void
        RegisterFluentValidation
        (this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UpdateTransactionStatusParamsValidator>();
    }

    public static void
        RegisterGenericRepo
        (this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }

    public static void
        RegisterUnitOfWork
        (this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void
    RegisterDbContext
        (this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = GenerateConnectionString();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
    }

    public static void
    ConfigureHangfire
    (this IServiceCollection services)
    {
        string connectionString = GenerateConnectionString();
        services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString));
        services.AddHangfireServer();
    }

    private static string
       GenerateConnectionString()
    {
        string connectionString = Environment.GetEnvironmentVariable("EPSDB");
        return connectionString;
    }
}
