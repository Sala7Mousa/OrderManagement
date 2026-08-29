using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application;
using OrderManagement.Application.Features.Orders.Commands.CreateOrder;
using OrderManagement.Application.Features.Products.Commands.CreateProduct;
using OrderManagement.Application.Features.Products.Commands.UpdateProduct;
using Xunit;

namespace OrderManagement.Tests;

public sealed class CommandValidationTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;

    public CommandValidationTests(ApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Create_product_command_rejects_negative_price_and_stock()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var command = new CreateProductCommand(
            new CreateProductRequest("Invalid product", "INVALID-001", -1m, -1));

        var exception = await Assert.ThrowsAsync<AppException>(
            () => sender.Send(command));

        Assert.Equal("validation_failed", exception.Code);
    }

    [Fact]
    public async Task Create_order_command_rejects_empty_items()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var command = new CreateOrderCommand(new CreateOrderRequest([]));

        var exception = await Assert.ThrowsAsync<AppException>(
            () => sender.Send(command));

        Assert.Equal("validation_failed", exception.Code);
    }

    [Fact]
    public async Task Update_product_command_rejects_request_without_fields()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var command = new UpdateProductCommand(
            Guid.NewGuid(),
            new UpdateProductRequest(null, null, null, null));

        var exception = await Assert.ThrowsAsync<AppException>(
            () => sender.Send(command));

        Assert.Equal("validation_failed", exception.Code);
    }
}
