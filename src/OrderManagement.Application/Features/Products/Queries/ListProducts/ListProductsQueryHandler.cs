using MediatR;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Application.Features.Products.Queries.ListProducts;

public sealed class ListProductsQueryHandler(IAppDbContext db) : IRequestHandler<ListProductsQuery, PagedResult<ProductDto>>
{
    public async Task<PagedResult<ProductDto>> Handle(ListProductsQuery query, CancellationToken ct)
    {
        var products = db.Products.AsNoTracking().Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            products = products.Where(product => product.Name.Contains(query.Name.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(query.Sku))
        {
            products = products.Where(product => product.Sku.Contains(query.Sku.Trim()));
        }

        var descending = query.SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);
        products = (query.SortBy.ToLowerInvariant(), descending) switch
        {
            ("price", false) => products.OrderBy(product => product.Price).ThenBy(product => product.Name),
            ("price", true) => products.OrderByDescending(product => product.Price).ThenBy(product => product.Name),
            ("name", true) => products.OrderByDescending(product => product.Name).ThenBy(product => product.Id),
            _ => products.OrderBy(product => product.Name).ThenBy(product => product.Id)
        };

        var total = await products.CountAsync(ct);
        var items = await products.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize)
            .Select(x => new ProductDto(x.Id, x.Name, x.Sku, x.Price, x.Stock, x.IsActive))
            .ToListAsync(ct);
        return new PagedResult<ProductDto>(items, query.Page, query.PageSize, total);
    }
}
