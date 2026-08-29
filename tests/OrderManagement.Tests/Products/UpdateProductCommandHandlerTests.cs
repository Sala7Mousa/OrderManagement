using Microsoft.EntityFrameworkCore;
using OrderManagement.Application;
using OrderManagement.Application.Features.Products.Commands.UpdateProduct;
using OrderManagement.Domain;
using OrderManagement.Infrastructure;
using Xunit;

namespace OrderManagement.Tests;

public sealed class UpdateProductCommandHandlerTests
{
    [Fact]
    public async Task Patch_updates_only_supplied_product_fields()
    {
        await using var db = CreateDb();
        var product = new Product
        {
            Name = "Original name",
            Sku = "SKU-001",
            Price = 10m,
            Stock = 5,
            IsActive = true
        };
        db.Products.Add(product);
        await db.SaveChangesAsync();

        var handler = new UpdateProductCommandHandler(db);
        var command = new UpdateProductCommand(
            product.Id,
            new UpdateProductRequest(null, 25m, null, null));

        var result = await handler.Handle(command, default);

        Assert.Equal("Original name", result.Name);
        Assert.Equal("SKU-001", result.Sku);
        Assert.Equal(25m, result.Price);
        Assert.Equal(5, result.Stock);
        Assert.True(result.IsActive);
    }

    private static AppDbContext CreateDb() => new(
        new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
}
