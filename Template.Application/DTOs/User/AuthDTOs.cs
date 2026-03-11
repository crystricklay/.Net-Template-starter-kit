namespace Template.Application.DTOs.User;

public class RegisterUserRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class LoginRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class AuthResponse
{
    public string AccessToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}