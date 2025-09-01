
using TaskManager.Application.DTOs.Security.User;

namespace TaskManager.Application.Interfaces.Services.Security
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginRequest request);
    }
}
