namespace OrderManagement.Application;

public sealed record CreateProductRequest(string Name, string Sku, decimal Price, int Stock, bool IsActive = true);
