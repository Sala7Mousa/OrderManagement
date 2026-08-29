using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Application.Features.Orders;

namespace OrderManagement.Application.Features.Orders.Queries.GetOrder;

public sealed class GetOrderQueryHandler(
    IAppDbContext db,
    ICurrentUser currentUser) : IRequestHandler<GetOrderQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderQuery query, CancellationToken ct)
    {
        var userId = currentUser.GetRequiredUserId();
        IQueryable<Order> orders = db.Orders.AsNoTracking().Include(x => x.Items).ThenInclude(x => x.Product);
        if (currentUser.Role != Roles.Admin) orders = orders.Where(x => x.CustomerId == userId);
        var order = await orders.SingleOrDefaultAsync(x => x.Id == query.Id, ct)
            ?? throw new AppException(404, "order_not_found", "Order was not found.");
        return OrderMapper.Map(order);
    }
}
