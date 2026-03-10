using MediatR;

namespace Template.Application.Commands.Users;

public record DeleteUserCommand(Guid Id) : IRequest;