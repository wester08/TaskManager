

using TaskManager.Application.DTOs.Task;
using TaskManager.Domain.Base;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces.Services
{
    public interface ITareaService
    {
        Task<OperationResult> GetAllTareaAsync();
        Task<OperationResult> GetTareaByIdAsync(int id);
        Task<OperationResult> CreateTareaAsync(TareaAddDto tareaAddDto);
        Task<OperationResult> UpdateTareaAsync(TareaUpdateDto tareaUpdateDto);
        Task<OperationResult> DeleteTareaAsync(int id);
        Task<OperationResult> FindByStatusAsync(string status);

        Task<OperationResult> FindByDueDateAsync(DateOnly date);


    }
}
