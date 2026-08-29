using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Application.Features.Orders;

namespace OrderManagement.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler(
    IAppDbContext db,
    ICurrentUser currentUser) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        var customerId = currentUser.GetRequiredUserId();
        var ids = command.Request.Items.Select(x => x.ProductId).ToArray();
        var products = await db.Products.Where(x => ids.Contains(x.Id)).ToDictionaryAsync(x => x.Id, ct);
        if (products.Count != ids.Length)
        {
            throw new AppException(400, "invalid_product", "One or more products do not exist.");
        }

        var order = new Order { CustomerId = customerId };
        foreach (var line in command.Request.Items)
        {
            var product = products[line.ProductId];
            if (!product.IsActive)
            {
                throw new AppException(400, "inactive_product", $"Product {product.Sku} is inactive.");
            }
            if (product.Stock < line.Quantity)
            {
                throw new AppException(409, "insufficient_stock", $"Insufficient stock for {product.Sku}.");
            }
            product.Stock -= line.Quantity;
            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = line.Quantity,
                UnitPrice = product.Price
            });
        }
        order.TotalAmount = order.Items.Sum(x => x.LineTotal);
        db.Orders.Add(order);
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new AppException(409, "inventory_conflict", "Inventory changed while the order was being created. Please retry.");
        }
        return OrderMapper.Map(order);
    }
}
