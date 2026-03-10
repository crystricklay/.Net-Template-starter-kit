using MediatR;
using Template.Application.DTOs.User;

namespace Template.Application.Commands.Users;

public record UpdateUserCommand(Guid Id, UpdateUserRequest User) : IRequest;