using OrderManagement.Infrastructure;
using Xunit;

namespace OrderManagement.Tests;

public sealed class DefaultUserSeedTests
{
    private readonly IdentityPasswordHashProvider _passwordHashProvider = new();

    [Fact]
    public void Admin_seed_hash_matches_documented_password()
    {
        Assert.True(_passwordHashProvider.Verify(
            DefaultUserSeeds.Admin.PasswordHash,
            "Admin123!"));
    }

    [Fact]
    public void Customer_seed_hash_matches_documented_password()
    {
        Assert.True(_passwordHashProvider.Verify(
            DefaultUserSeeds.Customer.PasswordHash,
            "Customer123!"));
    }
}
