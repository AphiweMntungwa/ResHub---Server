namespace ResHub.Services.Interfaces
{
    public interface IJwtTokenMiddleware
    {
        Task InvokeAsync(HttpContext context);
    }

}
