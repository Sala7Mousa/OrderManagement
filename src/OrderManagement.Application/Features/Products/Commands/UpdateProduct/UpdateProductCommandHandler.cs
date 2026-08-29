using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Features.Products;

namespace OrderManagement.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(IAppDbContext db) : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await db.Products.SingleOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new AppException(404, "product_not_found", "Product was not found.");
        if (!string.IsNullOrEmpty(command.Request.Name))
        {
            product.Name = command.Request.Name.Trim();
        }

        if (command.Request.Price.HasValue)
        {
            product.Price = command.Request.Price.Value;
        }

        if (command.Request.Stock.HasValue)
        {
            product.Stock = command.Request.Stock.Value;
        }

        if (command.Request.IsActive.HasValue)
        {
            product.IsActive = command.Request.IsActive.Value;
        }

        await db.SaveChangesAsync(ct);
        return ProductMapper.Map(product);
    }
}
