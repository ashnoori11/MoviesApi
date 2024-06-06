using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Actor.Queries;

public record SearchActorByNameQuery(string Query) : IRequest<Result<ActorSearchResultDto>>;

public class SearchActorByNameQueryHandler : IRequestHandler<SearchActorByNameQuery, Result<ActorSearchResultDto>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public SearchActorByNameQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion


    public async Task<Result<ActorSearchResultDto>> Handle(SearchActorByNameQuery request, CancellationToken cancellationToken)
    {
        var getRes = await _unitOfWork.GetQueryRepository<Domain.Entities.Actor>()
                .GetSpecificColumsByFilterAndOrderByAsync(a => a.Name.Contains(request.Query),
                a => new ActorSearchResultDto(a.Id,a.Name,a.Picture),
                cancellationToken);

        return Result<ActorSearchResultDto>
            .Success(getRes
            .OrderBy(a=>a.Name)
            .ToList());
    }
}
