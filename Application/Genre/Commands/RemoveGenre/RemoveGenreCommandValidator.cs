using FluentValidation;

namespace Application.Genre.Commands.RemoveGenre;

public class RemoveGenreCommandValidator : AbstractValidator<RemoveGenreCommand>
{
    public RemoveGenreCommandValidator()
    {
        RuleFor(a => a.Id)
            .NotEmpty().WithMessage("invalid Id for Genre")
            .NotNull().WithMessage("Genre Id can not be null or empty")
            .NotEqual(0).WithMessage("invalid Id for Genre");
    }
}
