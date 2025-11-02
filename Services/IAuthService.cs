using server.Models;

namespace server.Services;


public interface IAuthService
{
    string CreateToken(User user);
}