using MediatR;

namespace OrderManagement.Application.Features.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid Id) : IRequest<OrderDto>, ICustomerRequest;
