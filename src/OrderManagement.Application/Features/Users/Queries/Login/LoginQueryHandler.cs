using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Application.Features.Users.Queries.Login;

public sealed class LoginQueryHandler(
    IAppDbContext db,
    IPasswordHashProvider passwordHashProvider,
    IJwtTokenGenerator tokenGenerator) : IRequestHandler<LoginQuery, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginQuery query, CancellationToken ct)
    {
        var email = query.Request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == email, ct);
        if (user is null || !passwordHashProvider.Verify(user.PasswordHash, query.Request.Password))
        {
            throw new AppException(401, "invalid_credentials", "Invalid email or password.");
        }

        return new AuthResponse(tokenGenerator.Generate(user), user.Role);
    }
}
