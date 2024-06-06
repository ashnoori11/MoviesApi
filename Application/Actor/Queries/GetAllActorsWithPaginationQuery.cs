using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using Mapster;
using MediatR;

namespace Application.Actor.Queries;

public record GetAllActorsWithPaginationQuery(int PageNumber = 1,int PageSize = 10) : IRequest<Result<ActorSearchResultDto>>;

public class GetAllActorsWithPaginationQueryHandler : IRequestHandler<GetAllActorsWithPaginationQuery, Result<ActorSearchResultDto>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public GetAllActorsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<ActorSearchResultDto>> Handle(GetAllActorsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var getAll = await _unitOfWork.GetQueryRepository<Domain.Entities.Actor>().GetAllRowsNoTracking(request.PageNumber,request.PageSize,cancellationToken);
        return Result<ActorSearchResultDto>.Success(getAll.DataList.Adapt<List<ActorSearchResultDto>>(),getAll.TotalCount);
    }
}
