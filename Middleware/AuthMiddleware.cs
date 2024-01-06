namespace Auth.Middleware;

public class AuthMiddleware
{
    
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out
                var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key is missing");
            return;
        }

        var apiKey = _configuration.GetValue<string>("Authentication:ApiKey");
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid Api Key");
            return;
        }

        await _next(context);
    }
}