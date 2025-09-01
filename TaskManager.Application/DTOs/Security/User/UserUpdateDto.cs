
namespace TaskManager.Application.DTOs.Security.User
{
    public record UserUpdateDto
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool Active { get; set; }

    }
}
