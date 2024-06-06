using FluentValidation;

namespace Application.Movie.Commands.DeleteMovie;

public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
            
    }
}
