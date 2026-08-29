using FluentValidation;

namespace OrderManagement.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Request.Name)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(command => command.Request.Sku)
            .NotEmpty()
            .MaximumLength(64);
        RuleFor(command => command.Request.Price)
            .GreaterThanOrEqualTo(0);
        RuleFor(command => command.Request.Stock)
            .GreaterThanOrEqualTo(0);
    }
}
