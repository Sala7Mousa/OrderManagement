using MediatR;

namespace OrderManagement.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(CreateOrderRequest Request) : IRequest<OrderDto>, ICustomerRequest;
