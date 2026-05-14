using CRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRM.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http) => _http = http;

    private ClaimsPrincipal? User => _http.HttpContext?.User;

    public Guid UserId =>
        Guid.Parse(User?.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new UnauthorizedAccessException());

    public string Email =>
        User?.FindFirstValue(JwtRegisteredClaimNames.Email)
            ?? throw new UnauthorizedAccessException();

    public string Role =>
        User?.FindFirstValue(ClaimTypes.Role)
            ?? throw new UnauthorizedAccessException();

    public IEnumerable<string> Permissions =>
        User?.FindAll("permission").Select(c => c.Value)
            ?? Enumerable.Empty<string>();

    public bool HasPermission(string permission)
        => Permissions.Contains(permission);
}