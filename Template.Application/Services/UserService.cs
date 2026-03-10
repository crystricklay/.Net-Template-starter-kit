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

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(MapToResponse);
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return user is null ? null : MapToResponse(user);
    }

    public async Task<UserResponse?> GetByEmailAsync(string email)
    {
        var normalizedEmail = NormalizeEmail(email);
        var user = await _unitOfWork.Users.GetByEmailAsync(normalizedEmail);
        return user is null ? null : MapToResponse(user);
    }

    public async Task<Guid> CreateAsync(CreateUserRequest request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);

        var existing = await _unitOfWork.Users.GetByEmailAsync(normalizedEmail);
        if (existing is not null)
        {
            throw new InvalidOperationException($"User with email '{normalizedEmail}' already exists.");
        }

        var user = new User
        {
            Email = normalizedEmail
        };

        var id = await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user is null)
        {
            throw new KeyNotFoundException($"User {id} not found");
        }

        var normalizedEmail = NormalizeEmail(request.Email);

        var existing = await _unitOfWork.Users.GetByEmailAsync(normalizedEmail);
        if (existing is not null && existing.Id != id)
        {
            throw new InvalidOperationException($"User with email '{normalizedEmail}' already exists.");
        }

        user.Email = normalizedEmail;

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

    private static string NormalizeEmail(string email) =>
        email.Trim().ToLowerInvariant();

    private static UserResponse MapToResponse(User user) =>
        new()
        {
            Id = user.Id,
            Email = user.Email
        };
}