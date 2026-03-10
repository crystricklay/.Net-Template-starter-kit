using Template.Application.DTOs.User;

namespace Template.Application.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<UserResponse?> GetByIdAsync(Guid id);
    Task<UserResponse?> GetByEmailAsync(string email);
    Task<Guid> CreateAsync(CreateUserRequest request);
    Task UpdateAsync(Guid id, UpdateUserRequest request);
    Task DeleteAsync(Guid id);
}