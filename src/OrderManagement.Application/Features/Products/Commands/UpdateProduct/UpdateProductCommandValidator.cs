using FluentValidation;

namespace OrderManagement.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.Request)
            .Must(request =>
                request.Name is not null ||
                request.Price.HasValue ||
                request.Stock.HasValue ||
                request.IsActive.HasValue)
            .WithMessage("At least one product field must be provided.");
        RuleFor(command => command.Request.Name!)
            .NotEmpty()
            .MaximumLength(200)
            .When(command => command.Request.Name is not null);
        RuleFor(command => command.Request.Price)
            .GreaterThan(0)
            .When(command => command.Request.Price.HasValue);
        RuleFor(command => command.Request.Stock)
            .GreaterThan(0)
            .When(command => command.Request.Stock.HasValue);
    }
}
