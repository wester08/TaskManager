

using TaskManager.Application.DTOs.Security.User;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Extentions.Security
{
    public static class UserExtension
    {
        public static User ToDomainEntityCreate(this UserAddDto dto, int userCreationIdFromToken)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(UserAddDto), "UserAddDto cannot be null.");
            }
            return new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password,
                CreationDate = DateTime.UtcNow,
                UserCreateId = userCreationIdFromToken,
                Active = true,

            };
        }
        public static User ToDomainEntityUpdate(this UserUpdateDto dto, int userUpdateIdFromToken)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(UserUpdateDto), "UserUpdateDto cannot be null.");
            }


            return new User
            {
                IdUser = dto.IdUser,
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password,
                UpdateDate = DateTime.UtcNow,
                UserUpdateId = userUpdateIdFromToken,
                Active = dto.Active 


            };
        }

        public static UserDto ToDto(this User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "User cannot be null.");
            }
            return new UserDto
            {
                IdUser = entity.IdUser,
                UserName = entity.UserName,
                Email = entity.Email,
                Password = entity.Password,
                CreationDate = entity.CreationDate,
                UserCreateId = entity.UserCreateId,
                UpdateDate = entity.UpdateDate,
                UserUpdateId = entity.UserUpdateId,
                Active = entity.Active
            };
        }

        public static List<UserDto> ToDtoList(this IEnumerable<User> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), "User list cannot be null.");
            }
            return entities.Select(e => e.ToDto()).ToList();
        }

    }
}
