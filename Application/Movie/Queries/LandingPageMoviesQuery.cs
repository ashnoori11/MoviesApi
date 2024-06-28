using Application.Common.Models;
using Application.ViewModel;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Movie.Queries;

public record LandingPageMoviesQuery(int Take) : IRequest<Result<LandingPageViewModel>>;

public class LandingPageMoviesQueryHandler : IRequestHandler<LandingPageMoviesQuery, Result<LandingPageViewModel>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public LandingPageMoviesQueryHandler(IUnitOfWork unitOfWork)
    {
            _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<LandingPageViewModel>> Handle(LandingPageMoviesQuery request, CancellationToken cancellationToken)
    {
        LandingPageViewModel viewModel = new();

        var getIntheaterMovies = await _unitOfWork.MovieRepository.GetTopUpCommingMoviesAsync(request.Take, cancellationToken);
        viewModel.UpCommingReleases = MovieViewModel.ConvertDomainModelToViewModelList(getIntheaterMovies);

        var getNewMovies = await _unitOfWork.MovieRepository.GetTopInTheaterMoviesAsync(request.Take,cancellationToken);
        viewModel.InTheaters = MovieViewModel.ConvertDomainModelToViewModelList(getNewMovies);

        return Result<LandingPageViewModel>.Success(viewModel);
    }
}

