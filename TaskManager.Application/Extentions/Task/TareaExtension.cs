

using TaskManager.Application.DTOs.Task;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Extentions.Task
{
    public static class TareaExtension
    {

        public static Tarea ToDomainEntityAdd(this TareaAddDto tareaAddDto)
        {
            return new Tarea
            {
                Description = tareaAddDto.Description,
                DueDate = tareaAddDto.DueDate,
                Status = tareaAddDto.Status,
                AditionalData = tareaAddDto.AditionalData,
                Active = tareaAddDto.Active
            };
        }
        public static Tarea ToDomainEntityUpdate(this TareaUpdateDto tareaUpdateDto)
        {
            return new Tarea
            {
                Id = tareaUpdateDto.Id,
                Description = tareaUpdateDto.Description,
                DueDate = tareaUpdateDto.DueDate,
                Status = tareaUpdateDto.Status,
                AditionalData = tareaUpdateDto.AditionalData,
                Active = tareaUpdateDto.Active
            };
        }

        public static TareaUpdateDto ToDto(this Tarea entity)
        {
            return new TareaUpdateDto
            {
                Id = entity.Id,
                Description = entity.Description,
                DueDate = entity.DueDate,
                Status = entity.Status,
                AditionalData = entity.AditionalData,
                Active = entity.Active
            };
        }
    }
}

