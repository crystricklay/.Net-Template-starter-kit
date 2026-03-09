using Template.Application.DTOs.User;

namespace Template.Application.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllAsync();
    Task<UserDTO?> GetByIdAsync(Guid id);
    Task<UserDTO?> GetByEmailAsync(string email);
    Task<Guid> CreateAsync(UserDTO dto);
    Task UpdateAsync(Guid id, UserDTO dto);
    Task DeleteAsync(Guid id);
}