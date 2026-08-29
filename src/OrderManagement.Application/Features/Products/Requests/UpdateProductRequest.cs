namespace OrderManagement.Application;

public sealed record UpdateProductRequest(
    string? Name,
    decimal? Price,
    int? Stock,
    bool? IsActive);
