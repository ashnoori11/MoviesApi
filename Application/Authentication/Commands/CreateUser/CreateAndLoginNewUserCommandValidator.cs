using FluentValidation;

namespace Application.Authentication.Commands.CreateUser;

public class CreateAndLoginNewUserCommandValidator : AbstractValidator<CreateAndLoginNewUserCommand>
{
    public CreateAndLoginNewUserCommandValidator()
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
