using OrderManagement.Domain;

namespace OrderManagement.Application.Features.Products;

public static class ProductMapper
{
    public static ProductDto Map(Product product) => new(
        product.Id,
        product.Name,
        product.Sku,
        product.Price,
        product.Stock,
        product.IsActive);
}
