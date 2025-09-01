

using TaskManager.Application.Interfaces.Respositories.Security;
using TaskManager.Domain.Base;
using TaskManager.Domain.Entities;
using TaskManager.Persistence.Base;
using TaskManager.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Persistence.Repositories.Security
{
    public sealed class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly TaskManagerContext _context;

        public UserRepository(TaskManagerContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<OperationResult> AddAsync(User entity)
        {
            if (entity == null)
            {
                return OperationResult.Failure("User entity cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(entity.UserName))
            {
                return OperationResult.Failure("UserName cannot be null or empty.");
            }

            if (await _context.Users.AnyAsync(u => u.UserName == entity.UserName))
            {
                return OperationResult.Failure($"User with username '{entity.UserName}' already exists.");
            }

            return await base.AddAsync(entity);

        }


        public override async Task<OperationResult> UpdateAsync(User entity)
        {
            if (entity == null)
            {
                return OperationResult.Failure("User entity cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(entity.UserName))
            {
                return OperationResult.Failure("UserName cannot be null or empty.");
            }
            var existingUser = await _context.Users.FindAsync(entity.IdUser);
            if (existingUser == null)
            {
                return OperationResult.Failure($"User with ID '{entity.IdUser}' does not exist.");
            }
            existingUser.UserName = entity.UserName;
            existingUser.UpdateDate = entity.UpdateDate;
            existingUser.UserUpdateId = entity.UserUpdateId;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return OperationResult.Success("User updated successfully.", existingUser);
        }

    }

}
