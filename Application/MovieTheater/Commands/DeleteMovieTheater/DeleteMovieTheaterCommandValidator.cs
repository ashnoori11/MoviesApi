using FluentValidation;

namespace Application.MovieTheater.Commands.DeleteMovieTheater;

public class DeleteMovieTheaterCommandValidator : AbstractValidator<DeleteMovieTheaterCommand>
{
    public DeleteMovieTheaterCommandValidator()
    {
        RuleFor(a => a.MovieTheaterId)
          .NotNull()
          .NotEqual(0);
    }
}
