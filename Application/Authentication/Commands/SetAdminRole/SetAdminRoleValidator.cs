using FluentValidation;

namespace Application.Authentication.Commands.SetAdminRole;

public class SetAdminRoleValidator : AbstractValidator<SetAdminRoleCommand>
{
    public SetAdminRoleValidator()
    {
        RuleFor(a => a.UserId)
        .NotNull()
        .NotEmpty(); 
    }
}
