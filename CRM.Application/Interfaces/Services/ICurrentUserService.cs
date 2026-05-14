namespace CRM.Application.Interfaces.Services
{
    // reads JWT claims from HttpContext
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string Email { get; }
        string Role { get; }
        IEnumerable<string> Permissions { get; }
        bool HasPermission(string permission);
    }
}