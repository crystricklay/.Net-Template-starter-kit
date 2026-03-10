using MediatR;
using Template.Application.Commands.Users;
using Template.Application.Services.Interfaces;

namespace Template.Application.Handlers.Users;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserService _userService;

    public CreateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.CreateAsync(request.User);
    }
}