using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Features.Products.Queries.ListProducts;
using OrderManagement.Domain;
using OrderManagement.Infrastructure;
using Xunit;

namespace OrderManagement.Tests;

public sealed class ListProductsQueryHandlerTests
{
    [Fact]
    public async Task List_filters_active_products_by_name_and_sku_and_sorts_by_price()
    {
        await using var db = CreateDb();
        db.Products.AddRange(
            CreateProduct("Keyboard Basic", "KEY-001", 50m),
            CreateProduct("Keyboard Pro", "KEY-002", 100m),
            CreateProduct("Keyboard Hidden", "KEY-003", 1m, false),
            CreateProduct("Mouse", "MOUSE-001", 25m));
        await db.SaveChangesAsync();

        var handler = new ListProductsQueryHandler(db);
        var result = await handler.Handle(
            new ListProductsQuery("Keyboard", "KEY", "price", "desc", 1, 20),
            default);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(["Keyboard Pro", "Keyboard Basic"], result.Items.Select(item => item.Name));
        Assert.All(result.Items, item => Assert.True(item.IsActive));
    }

    [Fact]
    public async Task List_returns_pagination_metadata()
    {
        await using var db = CreateDb();
        db.Products.AddRange(
            CreateProduct("A", "A-001", 1m),
            CreateProduct("B", "B-001", 2m),
            CreateProduct("C", "C-001", 3m));
        await db.SaveChangesAsync();

        var handler = new ListProductsQueryHandler(db);
        var result = await handler.Handle(
            new ListProductsQuery(null, null, "name", "asc", 2, 2),
            default);

        Assert.Single(result.Items);
        Assert.Equal("C", result.Items[0].Name);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(2, result.Page);
        Assert.Equal(2, result.PageSize);
    }

    private static AppDbContext CreateDb() => new(
        new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Product CreateProduct(
        string name,
        string sku,
        decimal price,
        bool isActive = true) => new()
        {
            Name = name,
            Sku = sku,
            Price = price,
            Stock = 10,
            IsActive = isActive
        };
}
