using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Clock;
using AxisMart.Application.Shared.Data;
using AxisMart.Application.Shared.Generator;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Framework.Repository;
using AxisMart.Infra.Ecommerce.Clock;
using AxisMart.Infra.Ecommerce.Data;
using AxisMart.Infra.Ecommerce.Outbox;
using AxisMart.Infra.Ecommerce.Repositories;
using AxisMart.Infra.Ecommerce.Repositories.User;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AxisMart.Infra.Ecommerce;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, UtcDateTimeProvider>();

        AddPersistence(services, configuration);

        AddAuthentication(services, configuration);

        // AddAuthorization(services);

        AddBackgroundJobs(services, configuration);

        return services;
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Audience = configuration.GetSection("Authentication").GetValue<string>("Audience");
                        options.MetadataAddress = configuration.GetSection("Authentication").GetValue<string>("MetadataUrl")!;
                        options.RequireHttpsMetadata = configuration.GetSection("Authentication").GetValue<bool>("RequireHttpsMetadata");
                        options.TokenValidationParameters.ValidIssuer = configuration.GetSection("Authentication").GetValue<string>("Issuer");
                    });

        // services.Configure<Authentication.AuthenticationOptions>(configuration.GetSection("Authentication"));
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Ecommerce")!;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'Ecommerce' is missing or empty in configuration.");
        }

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        services.AddDbContext<AxisMartContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAdministratorRepository, AdministratorRepository>();
        services.AddScoped<ICredentialRepository, CredentialRepository>();
        services.AddScoped<IOneTimePasswordRepository, OneTimePasswordRepository>();
        services.AddScoped<IJsonWebTokenRepository, JsonWebTokenRepository>();
        
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<ITextMessageService, TextMessageService>();
        services.AddScoped<IJwtService, JwtTokenService>();
        services.AddScoped<IRsaKeyProvider, RsaKeyProvider>();
        services.AddScoped<IIdGenerator, IdGenerator>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AxisMartContext>());
    }

    private static void AddDatabase(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Master")!;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'QR' is missing or empty in configuration.");
        }

        var sqlScript = """
        IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Axismart')
        BEGIN
            CREATE DATABASE [Axismart];

            ALTER DATABASE [Axismart] SET 
                RECOVERY FULL,
                PAGE_VERIFY CHECKSUM,
                AUTO_UPDATE_STATISTICS ON,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF;

            ALTER DATABASE [Axismart] SET QUERY_STORE = ON;
        END
        """;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = new SqlCommand(sqlScript, connection);
        command.ExecuteNonQuery();

    }

    //private static void AddAuthorization(IServiceCollection services)
    //{
    //    services.AddScoped<AuthorizationService>();

    //    services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
    //}

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));

        services.AddQuartz();

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
    }
}
