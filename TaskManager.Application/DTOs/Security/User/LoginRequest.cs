
namespace TaskManager.Application.DTOs.Security.User
{
    public record LoginRequest
    {
        public string Email { get; init; }
        public string Password { get; init; }

    }
}
