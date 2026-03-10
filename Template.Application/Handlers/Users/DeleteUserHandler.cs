using MediatR;
using Template.Application.Commands.Users;
using Template.Application.Services.Interfaces;

namespace Template.Application.Handlers.Users;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserService _userService;

    public DeleteUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userService.DeleteAsync(request.Id);
    }
}