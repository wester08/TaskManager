using TaskManager.Application.DTOs.Security.User;
using TaskManager.Domain.Base;

namespace TaskManager.Application.Interfaces.Services.Security
{
    public interface IUserService
    {
        Task<OperationResult> GetAllUserAsync();
        Task<OperationResult> GetUserByIdAsync(int id);
        Task<OperationResult> GetUserByUserNameAsync(string UserName);
        Task<OperationResult> GetUserByEmailAsync(string Email);
        Task<OperationResult> CreateUserAsync(UserAddDto userAddDto, int userCreateIdFromToken);
        Task<OperationResult> UserUpdateAsync(UserUpdateDto userUpdateDto, int userCreateIdFromToken);

    }
}
