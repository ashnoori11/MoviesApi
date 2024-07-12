using FluentValidation;

namespace Application.Authentication.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(a => a.Email)
           .NotEmpty()
           .NotNull()
           .EmailAddress();

        RuleFor(a => a.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(6)
            .MaximumLength(25);
    }
}
