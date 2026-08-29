using FluentValidation;

namespace OrderManagement.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(command => command.Request.Items)
            .NotEmpty();
        RuleForEach(command => command.Request.Items).ChildRules(line =>
        {
            line.RuleFor(item => item.ProductId).NotEmpty();
            line.RuleFor(item => item.Quantity).GreaterThan(0);
        });
        RuleFor(command => command.Request.Items)
            .Must(items => items.Select(item => item.ProductId).Distinct().Count() == items.Count)
            .WithMessage("Duplicate product lines are not allowed.");
    }
}
