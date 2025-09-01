


using TaskManager.Application.DTOs.Security.User;
using TaskManager.Application.Extentions.Security;
using TaskManager.Application.Interfaces.Respositories.Security;
using TaskManager.Application.Interfaces.Services.Security;
using TaskManager.Domain.Base;
using TaskManager.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TaskManager.Application.Services.Security
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IConfiguration configuration;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            this.configuration = configuration;
        }


        public async Task<OperationResult> GetAllUserAsync()
        {
            OperationResult operationResult = new OperationResult();

            try {

                _logger.LogInformation("Retrieving all users from the repository.");
                var result = await _userRepository.GetAllAsync(ua => ua.Active);

                if (result.IsSuccess && result.Data is not null)
                {
                    var users = ((List<User>)result.Data).ToList();

                    _logger.LogInformation("Users retrieved successfully.");
                    operationResult = OperationResult.Success("Users retrieved successfully.", users);
                }
                else
                {
                    _logger.LogWarning("No users found.");
                    operationResult = OperationResult.Failure("No users found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all User: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while retrieving users.");

            }
            return operationResult;
        }

        public async Task<OperationResult> GetUserByIdAsync(int id)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation($"Retrieving user with ID: {id}");
                var result = await _userRepository.GetByIdAsync(id);
                if (result.IsSuccess && result.Data is not null)
                {
                    var user = (User)result.Data;
                    _logger.LogInformation("User retrieved successfully.");
                    operationResult = OperationResult.Success("User retrieved successfully.", user);
                }
                else
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    operationResult = OperationResult.Failure($"User with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user by ID: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while retrieving the user.");
            }
            return operationResult;
        }

        public async Task<OperationResult> GetUserByUserNameAsync(string UserName)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation($"Retrieving user with UserName: {UserName}");
                var result = await _userRepository.GetByNameAsync(UserName);
                if (result.IsSuccess && result.Data is not null)
                {
                    var users = ((List<User>)result.Data).ToList();
                    _logger.LogInformation("User retrieved successfully.");
                    operationResult = OperationResult.Success("User retrieved successfully.", users);
                }
                else
                {
                    _logger.LogWarning($"User with UserName {UserName} not found.");
                    operationResult = OperationResult.Failure($"User with UserName {UserName} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user by UserName: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while retrieving the user.");
            }
            return operationResult;
        }
        public async Task<OperationResult> GetUserByEmailAsync(string Email)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation($"Retrieving user with Email: {Email}");
                var result = await _userRepository.GetByEmailAsync(Email);
                if (result.IsSuccess && result.Data is not null)
                {
                    var user = (User)result.Data;
                    _logger.LogInformation("User retrieved successfully.");
                    operationResult = OperationResult.Success("User retrieved successfully.", user);
                }
                else
                {
                    _logger.LogWarning($"User with Email {Email} not found.");
                    operationResult = OperationResult.Failure($"User with Email {Email} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user by Email: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while retrieving the user.");
            }
            return operationResult;
        }
        public async Task<OperationResult> CreateUserAsync(UserAddDto userAddDto, int userCreateIdFromToken)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation("Creating a new user.");

                if (userAddDto == null)
                {
                    _logger.LogWarning("UserAddDto is null.");
                    return OperationResult.Failure("User data cannot be null.");
                }


                if (!string.IsNullOrEmpty(userAddDto.Password))
                {
                    userAddDto = userAddDto with
                    {
                        Password = BCrypt.Net.BCrypt.HashPassword(userAddDto.Password)
                    };
                }
                else
                {
                    return OperationResult.Failure("Password cannot be null or empty.");
                }

                var userEntity = userAddDto.ToDomainEntityCreate(userCreateIdFromToken);

                operationResult = await _userRepository.AddAsync(userEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while creating the user.");
            }

            return operationResult;
        }

         


        public async Task<OperationResult> UserUpdateAsync(UserUpdateDto userUpdateDto, int userCreateIdFromToken)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation("Updating user information.");
                if (userUpdateDto == null)
                {
                    _logger.LogWarning("UserUpdateDto is null.");
                    return OperationResult.Failure("User data cannot be null.");
                }
                var userEntity = userUpdateDto.ToDomainEntityUpdate(userCreateIdFromToken);
                operationResult = await _userRepository.UpdateAsync(userEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while updating the user.");
            }
            return operationResult;
        }
    }
}
