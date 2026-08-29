namespace OrderManagement.Application;

public sealed record OrderDto(Guid Id, Guid CustomerId, string Status, DateTimeOffset CreatedAt, decimal TotalAmount, IReadOnlyList<OrderItemDto> Items);
