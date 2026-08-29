using MediatR;

namespace OrderManagement.Application.Features.Products.Queries.ListProducts;

public sealed record ListProductsQuery(
    string? Name,
    string? Sku,
    string SortBy,
    string SortDirection,
    int Page,
    int PageSize) : IRequest<PagedResult<ProductDto>>, IAuthenticatedRequest;
