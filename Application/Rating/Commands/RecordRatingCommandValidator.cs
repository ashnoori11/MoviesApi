using FluentValidation;

namespace Application.Rating.Commands;

public class RecordRatingCommandValidator : AbstractValidator<RecordRatingCommand>
{
    public RecordRatingCommandValidator()
    {
        RuleFor(a => a.Email)
        .EmailAddress()
        .NotEmpty()
        .NotNull();

        RuleFor(a => a.Rating)
            .Must(a => a > 0 && a < 6)
            .NotNull();

        RuleFor(a => a.MovieId)
            .NotNull()
            .NotEmpty()
            .NotEqual(0);
    }
}
