using FluentValidation;

namespace OrderManagement.Application.Features.Users.Queries.Login;

public sealed class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(query => query.Request.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(query => query.Request.Password)
            .NotEmpty();
    }
}
