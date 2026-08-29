using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application;
using OrderManagement.Infrastructure;
using Xunit;

namespace OrderManagement.Tests;

public sealed class ApiIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    public ApiIntegrationTests(ApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Register_endpoint_persists_customer_in_database()
    {
        var client = _factory.CreateClient();
        var email = $"integration-{Guid.NewGuid():N}@example.com";
        var response = await client.PostAsJsonAsync("/api/auth/register", new RegisterRequest(email, "Customer123!"));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.True(await db.Users.AnyAsync(x => x.Email == email));
    }
}
