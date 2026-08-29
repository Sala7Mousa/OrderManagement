using Microsoft.EntityFrameworkCore;
using OrderManagement.Application;
using OrderManagement.Application.Features.Orders.Commands.CancelOrder;
using OrderManagement.Application.Features.Orders.Commands.CreateOrder;
using OrderManagement.Application.Features.Orders.Queries.GetOrder;
using OrderManagement.Domain;
using OrderManagement.Infrastructure;
using Xunit;

namespace OrderManagement.Tests;

public sealed class OrderHandlerTests
{
    [Fact]
    public async Task Create_calculates_total_and_deducts_stock()
    {
        await using var db = CreateDb(); var user = AddCustomer(db); var product = AddProduct(db, 12.50m, 5); await db.SaveChangesAsync();
        var handler = new CreateOrderCommandHandler(db, new FakeUser(user.Id));
        var result = await handler.Handle(new CreateOrderCommand(new CreateOrderRequest([new(product.Id, 2)])), default);
        Assert.Equal(25m, result.TotalAmount); Assert.Equal(3, product.Stock); Assert.Equal(12.50m, result.Items[0].UnitPrice);
    }

    [Fact]
    public async Task Create_rejects_insufficient_inventory_without_deducting_stock()
    {
        await using var db = CreateDb(); var user = AddCustomer(db); var product = AddProduct(db, 10m, 1); await db.SaveChangesAsync();
        var handler = new CreateOrderCommandHandler(db, new FakeUser(user.Id));
        var ex = await Assert.ThrowsAsync<AppException>(() => handler.Handle(new CreateOrderCommand(new CreateOrderRequest([new(product.Id, 2)])), default));
        Assert.Equal("insufficient_stock", ex.Code); Assert.Equal(1, product.Stock);
    }

    [Fact]
    public async Task Customer_cannot_read_another_customers_order()
    {
        await using var db = CreateDb(); var owner = AddCustomer(db); var other = AddCustomer(db, "other@example.com"); var order = new Order { CustomerId = owner.Id }; db.Orders.Add(order); await db.SaveChangesAsync();
        var handler = new GetOrderQueryHandler(db, new FakeUser(other.Id));
        var ex = await Assert.ThrowsAsync<AppException>(() => handler.Handle(new GetOrderQuery(order.Id), default));
        Assert.Equal(404, ex.StatusCode);
    }

    [Fact]
    public async Task Cancel_restores_inventory_and_rejects_repeat_attempt()
    {
        await using var db = CreateDb(); var user = AddCustomer(db); var product = AddProduct(db, 5m, 3); var order = new Order { CustomerId = user.Id, TotalAmount = 10m, Items = [new OrderItem { ProductId = product.Id, Product = product, Quantity = 2, UnitPrice = 5m }] }; db.Orders.Add(order); await db.SaveChangesAsync();
        var handler = new CancelOrderCommandHandler(db, new FakeUser(user.Id));
        await handler.Handle(new CancelOrderCommand(order.Id), default);
        Assert.Equal(5, product.Stock); await Assert.ThrowsAsync<AppException>(() => handler.Handle(new CancelOrderCommand(order.Id), default));
    }

    [Fact]
    public async Task Stored_unit_price_is_unchanged_after_product_price_update()
    {
        await using var db = CreateDb(); var user = AddCustomer(db); var product = AddProduct(db, 7m, 2); await db.SaveChangesAsync();
        var createHandler = new CreateOrderCommandHandler(db, new FakeUser(user.Id));
        var created = await createHandler.Handle(new CreateOrderCommand(new CreateOrderRequest([new(product.Id, 1)])), default); product.Price = 99m; await db.SaveChangesAsync();
        var getHandler = new GetOrderQueryHandler(db, new FakeUser(user.Id));
        var loaded = await getHandler.Handle(new GetOrderQuery(created.Id), default); Assert.Equal(7m, loaded.Items[0].UnitPrice);
    }

    private static AppDbContext CreateDb() => new(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    private static User AddCustomer(AppDbContext db, string email = "customer@example.com") { var u = new User { Email = email, PasswordHash = "hash", Role = Roles.Customer }; db.Users.Add(u); return u; }
    private static Product AddProduct(AppDbContext db, decimal price, int stock) { var p = new Product { Name = "Widget", Sku = Guid.NewGuid().ToString("N"), Price = price, Stock = stock }; db.Products.Add(p); return p; }
}
