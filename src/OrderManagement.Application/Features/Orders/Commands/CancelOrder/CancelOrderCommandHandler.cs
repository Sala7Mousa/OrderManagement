using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Application.Features.Orders;

namespace OrderManagement.Application.Features.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandHandler(
    IAppDbContext db,
    ICurrentUser currentUser) : IRequestHandler<CancelOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CancelOrderCommand command, CancellationToken ct)
    {
        var userId = currentUser.GetRequiredUserId();
        var order = await db.Orders.Include(x => x.Items).ThenInclude(x => x.Product)
            .SingleOrDefaultAsync(x => x.Id == command.Id && x.CustomerId == userId, ct)
            ?? throw new AppException(404, "order_not_found", "Order was not found.");
        if (order.Status != OrderStatus.Pending)
        {
            throw new AppException(409, "order_not_pending", "Only pending orders may be cancelled.");
        }
        foreach (var item in order.Items) item.Product!.Stock += item.Quantity;
        order.Status = OrderStatus.Cancelled;
        order.CancelledAt = DateTimeOffset.UtcNow;
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new AppException(409, "inventory_conflict", "Inventory changed during cancellation. Please retry.");
        }
        return OrderMapper.Map(order);
    }
}
