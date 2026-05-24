using AxisMart.Infra.Ecommerce;
using AxisMart.Prez.Ecommerce.Middleware;
using Microsoft.EntityFrameworkCore;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbcontext = scope.ServiceProvider.GetRequiredService<AxisMartContext>();

        dbcontext.Database.Migrate();
    }

    public static IApplicationBuilder UserRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }
}