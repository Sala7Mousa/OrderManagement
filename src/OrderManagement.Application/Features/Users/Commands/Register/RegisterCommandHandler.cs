using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;

namespace OrderManagement.Application.Features.Users.Commands.Register;

public sealed class RegisterCommandHandler(
    IAppDbContext db,
    IPasswordHashProvider passwordHashProvider,
    IJwtTokenGenerator tokenGenerator) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand command, CancellationToken ct)
    {
        var email = command.Request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(x => x.Email == email, ct))
        {
            throw new AppException(409, "email_exists", "An account already exists for this email.");
        }

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHashProvider.Hash(command.Request.Password),
            Role = Roles.Customer
        };
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return new AuthResponse(tokenGenerator.Generate(user), user.Role);
    }
}
