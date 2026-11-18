using System.ComponentModel.DataAnnotations;
using BugTracker.Api.Models;

namespace BugTracker.Api.Dtos;

public class LoginRequest
{
    [Required, MaxLength(100)] public string Login { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Role Role { get; set; }
}

public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public Role Role { get; set; }
}
