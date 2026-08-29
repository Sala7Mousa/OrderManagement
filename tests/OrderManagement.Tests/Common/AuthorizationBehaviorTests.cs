using OrderManagement.Application;
using OrderManagement.Application.Features.Orders.Commands.CreateOrder;
using OrderManagement.Application.Features.Products.Commands.CreateProduct;
using OrderManagement.Application.Features.Products.Queries.ListProducts;
using OrderManagement.Domain;
using Xunit;

namespace OrderManagement.Tests;

public sealed class AuthorizationBehaviorTests
{
    [Fact]
    public async Task Authenticated_request_rejects_missing_user()
    {
        var behavior = new AuthorizationBehavior<ListProductsQuery, PagedResult<ProductDto>>(
            new FakeUser(null));
        var query = new ListProductsQuery(null, null, "name", "asc", 1, 20);

        var exception = await Assert.ThrowsAsync<AppException>(() =>
            behavior.Handle(query, () => Task.FromResult(new PagedResult<ProductDto>([], 1, 20, 0)), default));

        Assert.Equal(401, exception.StatusCode);
    }

    [Fact]
    public async Task Admin_request_rejects_customer_role()
    {
        var behavior = new AuthorizationBehavior<CreateProductCommand, ProductDto>(
            new FakeUser(Guid.NewGuid(), Roles.Customer));
        var command = new CreateProductCommand(
            new CreateProductRequest("Product", "SKU-001", 10m, 1));

        var exception = await Assert.ThrowsAsync<AppException>(() =>
            behavior.Handle(command, () => Task.FromResult(CreateProductDto()), default));

        Assert.Equal(403, exception.StatusCode);
    }

    [Fact]
    public async Task Customer_request_rejects_admin_role()
    {
        var behavior = new AuthorizationBehavior<CreateOrderCommand, OrderDto>(
            new FakeUser(Guid.NewGuid(), Roles.Admin));
        var command = new CreateOrderCommand(
            new CreateOrderRequest([new OrderLineRequest(Guid.NewGuid(), 1)]));

        var exception = await Assert.ThrowsAsync<AppException>(() =>
            behavior.Handle(command, () => Task.FromResult(CreateOrderDto()), default));

        Assert.Equal(403, exception.StatusCode);
    }

    private static ProductDto CreateProductDto() =>
        new(Guid.NewGuid(), "Product", "SKU-001", 10m, 1, true);

    private static OrderDto CreateOrderDto() =>
        new(Guid.NewGuid(), Guid.NewGuid(), "Pending", DateTimeOffset.UtcNow, 0m, []);
}
