namespace CRM.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(Guid userId, string email, string role, IEnumerable<string> permissions);
        string GenerateRefreshToken();
        Guid? ValidateAccessToken(string token);
    }
}