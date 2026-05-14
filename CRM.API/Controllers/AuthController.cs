using CRM.Application.Features.Identity.Commands.Login;
using CRM.Application.Features.Identity.Commands.RefreshToken;
using CRM.Application.Features.Identity.Commands.RevokeToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;
    public AuthController(ISender sender) => _sender = sender;

    /// POST api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }

    /// POST api/auth/refresh
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenCommand command,
        CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }

    /// POST api/auth/revoke
    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> Revoke(
        [FromBody] RevokeTokenCommand command,
        CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}