using FluentValidation;

namespace Application.Actor.Commands.UpdateActor;

public class UpdateActorCommandValidator : AbstractValidator<UpdateActorCommand>
{
    public UpdateActorCommandValidator()
    {
        RuleFor(a => a.ActorId)
            .NotNull()
            .NotEmpty()
            .NotEqual(0)
            .WithMessage("invalid actor id");

        RuleFor(a => a.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .WithMessage("Invalid Actor Name");
         
        RuleFor(a => a.DateOfBirth)
        .NotEmpty()
        .NotNull()
        .NotEqual(DateTime.Now)
        .WithMessage("Invalid birthdate");

        RuleFor(a => a.Biography)
            .NotNull()
            .NotEmpty()
            .MaximumLength(849)
            .WithMessage("invalid length !");
    }
}
