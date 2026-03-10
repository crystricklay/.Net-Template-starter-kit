namespace Template.Application.DTOs.User;
public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
}
public class CreateUserRequest
{
    public string Email { get; set; } = default!;
}
public class UpdateUserRequest
{
    public string Email { get; set; } = default!;
}