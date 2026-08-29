using FluentValidation;

namespace OrderManagement.Application.Features.Products.Queries.ListProducts;

public sealed class ListProductsQueryValidator : AbstractValidator<ListProductsQuery>
{
    private static readonly string[] SortFields = ["name", "price"];
    private static readonly string[] SortDirections = ["asc", "desc"];

    public ListProductsQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThan(0);
        RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
        RuleFor(query => query.SortBy)
            .Must(value => SortFields.Contains(value, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortBy must be 'name' or 'price'.");
        RuleFor(query => query.SortDirection)
            .Must(value => SortDirections.Contains(value, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortDirection must be 'asc' or 'desc'.");
    }
}
