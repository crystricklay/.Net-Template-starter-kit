using MediatR;
using Template.Application.DTOs.User;
using Template.Application.Queries.Users;
using Template.Application.Services.Interfaces;

namespace Template.Application.Handlers.Users;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponse?>
{
    private readonly IUserService _userService;

    public GetUserByIdHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetByIdAsync(request.Id);
    }
}