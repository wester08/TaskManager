
using TaskManager.Domain.Entities;

namespace TaskManager.Security.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
