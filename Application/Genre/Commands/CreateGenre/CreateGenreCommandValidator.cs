using FluentValidation;

namespace Application.Genre.Commands.CreateGenre;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(a => a.Name)
        .NotEmpty().WithMessage("invalid value for genre title")
        .NotNull().WithMessage("genre title must contains 1 - 150 characters")
        .MaximumLength(140).WithMessage("genre title is too long")
        .MinimumLength(3).WithMessage("genre title is too short");
    }
}
