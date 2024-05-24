using FluentValidation;

namespace Application.MovieTheater.Commands.UpdateMovieTheater;

public class UpdateMovieTheaterCommandValidator : AbstractValidator<UpdateMovieTheaterCommand>
{
    public UpdateMovieTheaterCommandValidator()
    {
        RuleFor(a=>a.MovieTheaterId)
            .NotNull()
            .NotEqual(0);

        RuleFor(a => a.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(a => a.Latitude)
            .ExclusiveBetween(-90, 90);

        RuleFor(a => a.Longitude)
            .ExclusiveBetween(-180, 180);
    }
}
