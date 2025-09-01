
namespace TaskManager.Application.DTOs.Security.User
{
    public record UserDto
    {
        public int IdUser { get; init; }
        public string UserName { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }
        public DateTime CreationDate { get; init; }
        public int UserCreateId { get; init; }
        public DateTime? UpdateDate { get; init; }
        public int? UserUpdateId { get; init; }
        public bool Active { get; init; } = true;





    }
}
