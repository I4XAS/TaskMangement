
namespace server.DTOs;

public class RegisterDto
{
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;

}