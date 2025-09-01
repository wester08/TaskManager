using TaskManager.Application.DTOs.Security.User;
using TaskManager.Application.Interfaces.Respositories.Security;
using TaskManager.Application.Interfaces.Services.Security;
using TaskManager.Domain.Entities; 
using TaskManager.Security.Interfaces.Security;

namespace TaskManager.Application.Services.Security
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private readonly TimeSpan _tokenExpiration = TimeSpan.FromHours(1);

        public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtGenerator)
        {
            _userRepository = userRepository;
            _jwtGenerator = jwtGenerator;
        }


        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                    throw new ArgumentException("Email and password must be provided.");

                var result = await _userRepository.GetByEmailAsync(request.Email);
                if (!result.IsSuccess || result.Data is not User user)
                    throw new UnauthorizedAccessException("Invalid credentials.");

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    throw new UnauthorizedAccessException("Invalid credentials.");

                var token = _jwtGenerator.GenerateToken(user);


                Console.WriteLine($"Generated token for user {user.UserName}: {token}");

                return new AuthResult
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.Add(_tokenExpiration)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                throw;
            }
        }
    }
}
