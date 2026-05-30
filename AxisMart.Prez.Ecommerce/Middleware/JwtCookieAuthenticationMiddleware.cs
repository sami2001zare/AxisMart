using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AxisMart.Prez.Ecommerce.Middleware;

public class JwtCookieAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenValidationParameters _tokenValidationParams;

    public JwtCookieAuthenticationMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParams)
    {
        _next = next;
        _tokenValidationParams = tokenValidationParams;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies["jwt_token"];
        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParams, out _);
                context.User = principal; // Set user for this request
            }
            catch
            {
                // Token invalid – clear cookie
                // context.Response.Cookies.Delete("jwt_token");
            }
        }

        await _next(context);
    }
}