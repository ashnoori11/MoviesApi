using FluentValidation;

namespace Application.MovieTheater.Commands.CreateMovieTheater;

public class CreateMovieTheaterCommandValidator : AbstractValidator<CreateMovieTheaterCommand>
{
    public CreateMovieTheaterCommandValidator()
    {
        RuleFor(a => a.Name)
        .NotNull()
        .NotEmpty()
        .MaximumLength(75);

        RuleFor(a => a.Latitude)
            .ExclusiveBetween(-90,90);

        RuleFor(a => a.Longitude)
            .ExclusiveBetween(-180,180);
    }
}
