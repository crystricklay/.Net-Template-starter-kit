using MediatR;
using Template.Application.DTOs.User;

namespace Template.Application.Queries.Users;

public record GetUserByIdQuery(Guid Id) : IRequest<UserResponse?>;