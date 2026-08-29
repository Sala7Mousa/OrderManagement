using MediatR;

namespace OrderManagement.Application.Features.Orders.Queries.ListOrders;

public sealed record ListOrdersQuery(int Page, int PageSize) : IRequest<PagedResult<OrderDto>>, IAuthenticatedRequest;
