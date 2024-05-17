using FluentValidation;

namespace Application.Actor.Commands.CreateActor;

public class CreateActorCommandValidator : AbstractValidator<CreateActorCommand>
{
    public CreateActorCommandValidator()
    {
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

        RuleFor(a => a.Picture)
            .NotNull()
            .NotEmpty()
            .WithMessage("image is required");
    }
}
