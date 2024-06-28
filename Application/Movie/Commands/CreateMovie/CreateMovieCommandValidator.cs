using FluentValidation;

namespace Application.Movie.Commands.CreateMovie;

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(a => a.Title)
            .NotEmpty()
            .NotNull();

        RuleFor(a => a.Poster)
            .NotNull();

        RuleFor(a => a.GenresIds)
            .NotEmpty()
            .NotNull();

        //RuleFor(a => a.MovieTheatersIds)
        //    .NotEmpty()
        //    .NotNull();

        RuleFor(a => a.Actors)
            .NotEmpty()
            .NotNull();

        RuleFor(a => a.Summery)
            .MaximumLength(1000);

        RuleFor(a => a.Trailer)
            .MaximumLength(700);
    }
}
