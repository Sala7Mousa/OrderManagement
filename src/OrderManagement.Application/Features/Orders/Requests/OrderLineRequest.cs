namespace OrderManagement.Application;

public sealed record OrderLineRequest(Guid ProductId, int Quantity);
