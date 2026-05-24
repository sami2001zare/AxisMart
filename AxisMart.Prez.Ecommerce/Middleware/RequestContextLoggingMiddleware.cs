using Serilog.Context;

namespace AxisMart.Prez.Ecommerce.Middleware;

public class RequestContextLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext)))
        {
            return _next(httpContext);
        }
    }

    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    private static string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}
