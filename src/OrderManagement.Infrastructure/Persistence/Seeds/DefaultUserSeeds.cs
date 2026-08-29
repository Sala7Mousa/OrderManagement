using OrderManagement.Domain;

namespace OrderManagement.Infrastructure;

public static class DefaultUserSeeds
{
    public static User Admin => new()
    {
        Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        Email = "admin@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAECw/ZzlNJ/B6j1d7ugtm38fQsUxoGTixdLETl5wA3qM9YkF0fCiVOFvuTG2px1dNUQ==",
        Role = Roles.Admin
    };

    public static User Customer => new()
    {
        Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
        Email = "customer@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEIOwbny7HNAAGVK0n/ZJwqWO2PaHR1QxDSZF2BeQisND6QpYH41JnOT5ftVUAQ7sSw==",
        Role = Roles.Customer
    };
}
