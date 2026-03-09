using Template.Application.DTOs.User;
using Template.Application.Services.Interfaces;
using Template.Domain.Entities;
using Template.Domain.Interfaces.UnitOfWork;

namespace Template.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDTO?> GetByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

     public async Task<UserDTO?> GetByEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        return user is null ? null : MapToDto(user);
    }

    public async Task<Guid> CreateAsync(UserDTO dto)
    {
        var user = new User
        {
            Email = dto.Email
        };

        var id = await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task UpdateAsync(Guid id, UserDTO dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user is null)
        {
            throw new KeyNotFoundException($"User {id} not found");
        }

        user.Email = dto.Email;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user is null)
        {
            return;
        }

        await _unitOfWork.Users.DeleteAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    private static UserDTO MapToDto(User user) =>
        new()
        {
            Id = user.Id,
            Email = user.Email
        };
}