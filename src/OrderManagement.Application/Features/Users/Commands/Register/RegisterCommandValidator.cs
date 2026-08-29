using FluentValidation;

namespace OrderManagement.Application.Features.Users.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(command => command.Request.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(command => command.Request.Password)
            .MinimumLength(8);
    }
}
