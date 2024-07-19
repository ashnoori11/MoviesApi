using FluentValidation;

namespace Application.Authentication.Commands.RemoveAdminRole;

public class RemoveAdminRoleCommandValidator : AbstractValidator<RemoveAdminRoleCommand>
{
    public RemoveAdminRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
        .NotNull()
        .NotEmpty();
    }
}
