using MediatR;
using Template.Application.Commands.Users;
using Template.Application.Services.Interfaces;

namespace Template.Application.Handlers.Users;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserService _userService;

    public UpdateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await _userService.UpdateAsync(request.Id, request.User);
    }
}