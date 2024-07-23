namespace ResHub.Services.Interfaces
{
    public interface IJwtTokenService
    {
            string GenerateToken(string userId);
    }
}
