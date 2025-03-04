namespace ResHub.Services.Interfaces
{
    public interface IJwtTokenService
    {
            Task<string> GenerateToken(string userId, int residenceId);
    }
}
