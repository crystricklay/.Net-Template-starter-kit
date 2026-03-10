using MediatR;
using Template.Application.DTOs.User;

namespace Template.Application.Commands.Users;

public record CreateUserCommand(CreateUserRequest User) : IRequest<Guid>;