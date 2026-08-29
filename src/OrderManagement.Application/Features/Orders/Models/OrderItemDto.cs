namespace OrderManagement.Application;

public sealed record OrderItemDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal LineTotal);
