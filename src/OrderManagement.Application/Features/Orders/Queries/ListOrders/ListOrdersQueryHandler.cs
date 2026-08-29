using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Application.Features.Orders;

namespace OrderManagement.Application.Features.Orders.Queries.ListOrders;

public sealed class ListOrdersQueryHandler(
    IAppDbContext db,
    ICurrentUser currentUser) : IRequestHandler<ListOrdersQuery, PagedResult<OrderDto>>
{
    public async Task<PagedResult<OrderDto>> Handle(ListOrdersQuery query, CancellationToken ct)
    {
        var userId = currentUser.GetRequiredUserId();
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        IQueryable<Order> ordersQuery = db.Orders
            .AsNoTracking()
            .Include(order => order.Items)
            .ThenInclude(item => item.Product)
            .AsSplitQuery();
        if (currentUser.Role != Roles.Admin)
        {
            ordersQuery = ordersQuery.Where(order => order.CustomerId == userId);
        }

        ordersQuery = ordersQuery
            .OrderByDescending(order => order.CreatedAt)
            .ThenByDescending(order => order.Id);
        var total = await ordersQuery.CountAsync(ct);
        var orders = await ordersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<OrderDto>(
            orders.Select(OrderMapper.Map).ToList(),
            page,
            pageSize,
            total);
    }
}
