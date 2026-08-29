using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application;
using OrderManagement.Application.Features.Users.Commands.Register;
using OrderManagement.Application.Features.Users.Queries.Login;

namespace OrderManagement.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
    {
        return Ok(await sender.Send(new RegisterCommand(request), ct));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        return Ok(await sender.Send(new LoginQuery(request), ct));
    }
}
