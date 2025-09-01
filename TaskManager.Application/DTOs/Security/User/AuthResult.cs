

namespace TaskManager.Application.DTOs.Security.User
{
    public record  AuthResult
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

    }
}
