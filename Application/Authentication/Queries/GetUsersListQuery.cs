using Application.Common.Models;
using Application.Dtos;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Queries;

public class GetUsersListQuery : IRequest<Result<UserDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetUsersListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUsersListQuery, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<UserDto>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
    {
        var getResult = await _unitOfWork
            .GetQueryRepository<IdentityUser>()
            .GetAllRowsNoTracking(request.PageNumber, request.PageSize, cancellationToken);

        List<UserDto> model = new();
        foreach (var item in getResult.DataList)
        {
            model.Add(new UserDto { Id = item.Id, Email = item.Email });
        }

        return Result<UserDto>.Success(model);
    }
}