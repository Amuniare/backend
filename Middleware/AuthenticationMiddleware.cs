using System.Text.Json;

namespace UserManagementAPI.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;
    private readonly string _validToken = "techhive-api-key-2024";

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for root path
        if (context.Request.Path == "/")
        {
            await _next(context);
            return;
        }

        // Check for Authorization header
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            _logger.LogWarning("Request missing Authorization header");
            await UnauthorizedResponse(context, "Authorization header is missing");
            return;
        }

        var token = context.Request.Headers["Authorization"].ToString();

        // Remove "Bearer " prefix if present
        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = token.Substring(7);
        }

        // Validate token
        if (token != _validToken)
        {
            _logger.LogWarning("Invalid token provided");
            await UnauthorizedResponse(context, "Invalid token");
            return;
        }

        await _next(context);
    }

    private static Task UnauthorizedResponse(HttpContext context, string message)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = "Unauthorized",
            message = message
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}
