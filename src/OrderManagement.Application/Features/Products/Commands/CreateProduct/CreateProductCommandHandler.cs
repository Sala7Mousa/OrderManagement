using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;
using OrderManagement.Application.Features.Products;

namespace OrderManagement.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(IAppDbContext db) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var request = command.Request;
        var sku = request.Sku.Trim().ToUpperInvariant();
        if (await db.Products.AnyAsync(x => x.Sku == sku, ct))
        {
            throw new AppException(409, "sku_exists", "SKU already exists.");
        }

        var product = new Product
        {
            Name = request.Name.Trim(),
            Sku = sku,
            Price = request.Price,
            Stock = request.Stock,
            IsActive = request.IsActive
        };
        db.Products.Add(product);
        await db.SaveChangesAsync(ct);
        return ProductMapper.Map(product);
    }
}
