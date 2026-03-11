using System.Security.Cryptography;
using Template.Application.DTOs.User;
using Template.Application.Services.Interfaces;
using Template.Domain.Entities;
using Template.Domain.Interfaces.UnitOfWork;

namespace Template.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserRequest request)
    {
        var email = NormalizeEmail(request.Email);

        var existing = await _unitOfWork.Users.GetByEmailAsync(email);
        if (existing is not null)
        {
            throw new InvalidOperationException($"User with email '{email}' already exists.");
        }

        var passwordHash = HashPassword(request.Password);

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1) // keep in sync with JwtOptions
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = NormalizeEmail(request.Email);

        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }

    private static string NormalizeEmail(string email) =>
        email.Trim().ToLowerInvariant();

    private static string HashPassword(string password)
    {
        // simple PBKDF2; for prod you’d parameterize iterations/salt size
        using var deriveBytes = new Rfc2898DeriveBytes(password, 16, 100_000, HashAlgorithmName.SHA256);
        var salt = deriveBytes.Salt;
        var key = deriveBytes.GetBytes(32);
        var combined = new byte[1 + salt.Length + key.Length];
        combined[0] = 0; // version
        Buffer.BlockCopy(salt, 0, combined, 1, salt.Length);
        Buffer.BlockCopy(key, 0, combined, 1 + salt.Length, key.Length);
        return Convert.ToBase64String(combined);
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var data = Convert.FromBase64String(storedHash);
        var version = data[0];
        if (version != 0) return false;

        var salt = new byte[16];
        Buffer.BlockCopy(data, 1, salt, 0, salt.Length);

        var storedKey = new byte[32];
        Buffer.BlockCopy(data, 1 + salt.Length, storedKey, 0, storedKey.Length);

        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var computedKey = deriveBytes.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(storedKey, computedKey);
    }
}