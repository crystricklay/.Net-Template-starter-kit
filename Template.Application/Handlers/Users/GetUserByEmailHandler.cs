using MediatR;
using Template.Application.DTOs.User;
using Template.Application.Queries.Users;
using Template.Application.Services.Interfaces;

namespace Template.Application.Handlers.Users;

public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserResponse?>
{
    private readonly IUserService _userService;

    public GetUserByEmailHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserResponse?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetByEmailAsync(request.Email);
    }
}