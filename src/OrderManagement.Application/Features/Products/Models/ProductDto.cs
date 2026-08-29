namespace OrderManagement.Application;

public sealed record ProductDto(Guid Id, string Name, string Sku, decimal Price, int Stock, bool IsActive);
