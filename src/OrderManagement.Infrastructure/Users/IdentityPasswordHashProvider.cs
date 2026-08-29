using Microsoft.AspNetCore.Identity;
using OrderManagement.Application;
using OrderManagement.Domain;

namespace OrderManagement.Infrastructure;

public sealed class IdentityPasswordHashProvider : IPasswordHashProvider
{
    private readonly PasswordHasher<User> _hasher = new();
    public string Hash(string password) => _hasher.HashPassword(null!, password);
    public bool Verify(string hash, string password) => _hasher.VerifyHashedPassword(null!, hash, password) != PasswordVerificationResult.Failed;
}
