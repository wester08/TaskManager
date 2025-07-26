

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Extentions.Task;
using TaskManager.Application.Interfaces.Respositories.Task;
using TaskManager.Application.Interfaces.Services;
using TaskManager.Domain.Base;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services.Task
{
    public class TareaService : ITareaService
    {
        private readonly ITareaRepository _tareaRepository;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public TareaService(ITareaRepository tareaRepository, 
                            ILogger<TareaService> logger, 
                            IConfiguration configuration)
        {
            _tareaRepository = tareaRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GetAllTareaAsync()
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation("Retrieving all tareas from the repository");
                var result = await _tareaRepository.GetAllAsync(d => d.Active);

                if (result.IsSuccess && result.Data is not null) 
                    {
                    var tareas = ((List<Tarea>)result.Data).ToList();

                    _logger.LogInformation("Task retrieved successfully.");
                    operationResult = OperationResult.Success("Tareas retrived successfully", tareas);


                    }
                else
                {
                    _logger.LogWarning("No task found.");
                    operationResult = OperationResult.Failure("No Task found");

                }
                _logger.LogInformation("Successfully retrieved task");

            }
            catch (Exception ex)
            {
                _logger.LogError($" Error retrieving all task: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error ocurred while retrieving task");
                
            }
            return operationResult;
        }

        public async Task<OperationResult> GetTareaByIdAsync(int id)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation($"Retrieving task with id: {id}");
                var result = await _tareaRepository.GetByIdAsync(id);
                if (result.IsSuccess && result.Data is not null)
                {
                    var tareas = result.Data as Tarea;

                    operationResult = OperationResult.Success("task retrived successfully", tareas);


                }
                else
                {
                    operationResult = OperationResult.Failure("No Task found");

                }
                _logger.LogInformation("Successfully retrieved task");

            }
            catch (Exception ex)
            {
                _logger.LogError($" Error retrieving task: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error ocurred while retrieving task");

            }
            return operationResult;

        }

        public async Task<OperationResult> CreateTareaAsync(TareaAddDto tareaAddDto)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation($"Adding task with description: {tareaAddDto.Description}");
                if( tareaAddDto == null)
                {
           
                    operationResult = OperationResult.Failure("tareaAddDto is null");
                    return operationResult;
                }

                operationResult = await _tareaRepository.AddAsync(tareaAddDto.ToDomainEntityAdd());

            _logger.LogInformation("Successfully added a new Task");
        }

            catch (Exception ex)
            {
                _logger.LogError($"Error adding a new task: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while adding task.");

            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateTareaAsync(TareaUpdateDto tareaUpdateDto)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation($"Updating task with ID {tareaUpdateDto.Id} in the repository.");
                if (tareaUpdateDto == null)
                {
                    operationResult = OperationResult.Failure("tareaUpdateDto is null");
                    return operationResult;
                }


                operationResult = await _tareaRepository.UpdateAsync(tareaUpdateDto.ToDomainEntityUpdate());

                _logger.LogInformation("Successfully updated task.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating task with ID {tareaUpdateDto.Id}: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while updating task.");
            }
            return operationResult;

        }

        public async Task<OperationResult> DeleteTareaAsync(int id)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation($"Retrieving task with id: {id}");
                var result = await _tareaRepository.DeleteAsync(id);
                if (result.IsSuccess && result.Data is not null)
                {
                    var tareas = result.Data as Tarea;

                    operationResult = OperationResult.Success("Task removed successfully", tareas);


                }
                else
                {
                    operationResult = OperationResult.Failure("No Task found");

                }
                _logger.LogInformation("Successfully retrieved task");

            }
            catch (Exception ex)
            {
                _logger.LogError($" Error retrieving task: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error ocurred while retrieving task");

            }

            return operationResult;
        }

        public async Task<OperationResult> FindByStatusAsync(string status)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation($"Retrieving task with Status: {status}");
                var result = await _tareaRepository.GetByAsync(s => s.Status, status);
                if (result.IsSuccess && result.Data is not null)
                {
                    var tareas = ((List<Tarea>)result.Data).ToList();

                    operationResult = OperationResult.Success("tasks retrived successfully", tareas);


                }
                else
                {
                    operationResult = OperationResult.Failure("No Tasks found");

                }
                _logger.LogInformation("Successfully retrieved tasks");

            }
            catch (Exception ex)
            {
                _logger.LogError($" Error retrieving tasks: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error ocurred while retrieving tasks");

            }
            return operationResult;
        }

        public async Task<OperationResult> FindByDueDateAsync(DateOnly date)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation($"Retrieving task with date: {date}");
                var result = await _tareaRepository.GetByAsync(s => s.DueDate, date);
                if (result.IsSuccess && result.Data is not null)
                {
                    var tareas = ((List<Tarea>)result.Data).ToList();

                    operationResult = OperationResult.Success("tasks retrived successfully", tareas);


                }
                else
                {
                    operationResult = OperationResult.Failure("No Tasks found");

                }
                _logger.LogInformation("Successfully retrieved tasks");

            }
            catch (Exception ex)
            {
                _logger.LogError($" Error retrieving tasks: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error ocurred while retrieving tasks");

            }
            return operationResult;
        }
    }
}
