
namespace TaskManager.Application.DTOs.Security.User
{
    public record UserAddDto
    {
        public string UserName { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }

    }
}
