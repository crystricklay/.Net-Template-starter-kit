using MediatR;
using Template.Application.DTOs.User;
using Template.Application.Queries.Users;
using Template.Application.Services.Interfaces;

namespace Template.Application.Handlers.Users;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDTO>>
{
    private readonly IUserService _userService;

    public GetUsersHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IEnumerable<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetAllAsync();
    }
}