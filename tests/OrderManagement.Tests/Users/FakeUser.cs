using OrderManagement.Application;
using OrderManagement.Domain;

namespace OrderManagement.Tests;

public sealed class FakeUser(Guid? id, string? role = Roles.Customer) : ICurrentUser
{
    public Guid? Id => id;
    public string? Role => role;
}
