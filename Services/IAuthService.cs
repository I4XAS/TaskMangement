using server.Models;

namespace server.IAuthService;
public interface ItokenService
{
    string CreateToken(User user);
}