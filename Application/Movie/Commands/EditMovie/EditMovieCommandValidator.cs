using FluentValidation;

namespace Application.Movie.Commands.EditMovie;

public class EditMovieCommandValidator : AbstractValidator<EditMovieCommand>
{
    public EditMovieCommandValidator()
    {
        RuleFor(a => a.Title)
           .NotEmpty()
           .NotNull();

        RuleFor(a => a.GenresIds)
                .NotEmpty()
                .NotNull();

        RuleFor(a => a.Actors)
                .NotEmpty()
                .NotNull();

        RuleFor(a => a.Summery)
                .MaximumLength(1000);

        RuleFor(a => a.Trailer)
                .MaximumLength(700);
    }
}
