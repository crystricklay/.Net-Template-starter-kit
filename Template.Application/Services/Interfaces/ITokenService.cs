using Template.Domain.Entities;

namespace Template.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}