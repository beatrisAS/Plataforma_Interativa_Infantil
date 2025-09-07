using Plataforma.API.Models;

namespace Plataforma.API.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
