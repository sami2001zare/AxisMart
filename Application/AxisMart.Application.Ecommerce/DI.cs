using AxisMart.Application.Shared.Behaviors;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AxisMart.Application.Ecommerce;

public static class DI
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DI).Assembly);

            //configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));

            //configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));

            // configuration.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DI).Assembly);

        return services;
    }
}