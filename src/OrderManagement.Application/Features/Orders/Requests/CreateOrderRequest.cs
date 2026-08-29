namespace OrderManagement.Application;

public sealed record CreateOrderRequest(IReadOnlyList<OrderLineRequest> Items);
