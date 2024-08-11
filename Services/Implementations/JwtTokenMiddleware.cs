using ResHub.Services.Interfaces;

namespace ResHub.Services.Implementations
{
    public class JwtTokenMiddleware: IJwtTokenMiddleware
    {
            private readonly RequestDelegate _next;

            public JwtTokenMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                // Extract JWT token from the cookie
                if (context.Request.Cookies.TryGetValue("jwt-token", out var token))
                {
                    // Set the Authorization header with the Bearer token
                    context.Request.Headers["Authorization"] = $"Bearer {token}";
                }

                // Call the next middleware in the pipeline
                await _next(context);
            }

    }
}
