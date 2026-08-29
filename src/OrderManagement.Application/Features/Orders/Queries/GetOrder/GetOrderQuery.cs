using MediatR;

namespace OrderManagement.Application.Features.Orders.Queries.GetOrder;

public sealed record GetOrderQuery(Guid Id) : IRequest<OrderDto>, IAuthenticatedRequest;
