

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Extentions.Task;
using TaskManager.Application.Interfaces.Factories;
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
        private readonly ITareaFactory _tareaFactory;

        public TareaService(ITareaRepository tareaRepository,
                            ILogger<TareaService> logger,
                            IConfiguration configuration,
                            ITareaFactory tareaFactory)
        {
            _tareaRepository = tareaRepository;
            _logger = logger;
            _configuration = configuration;
            _tareaFactory = tareaFactory;
        }

        //Func para calculos derivados.
        Func<Tarea, int> calculateDaysLeft = task
             =>
        {
            var DueDateTime = task.DueDate.ToDateTime(TimeOnly.MinValue);
            var days = (DueDateTime - DateTime.Now).Days;

            if (task.Status == "Completado" || days < 0)
            {
                return 0;
            }
            else
            {

            }
            return days;
        };
        public async Task<OperationResult> GetAllTareaAsync()
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation("Retrieving all tareas from the repository");
                var result = await _tareaRepository.GetAllAsync(d => d.Active);

                if (result.IsSuccess && result.Data is not null)
                {
                    var dto = ((List<Tarea>)result.Data).Select(t => new TareaDto
                    {
                        Id = t.Id,
                        Description = t.Description,
                        DueDate = t.DueDate,
                        Status = t.Status,
                        AditionalData = t.AditionalData,
                        Active = t.Active,
                        DaysLeft = calculateDaysLeft(t)
                    });


                    var tareas = dto.ToList();

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
                    var dto = new TareaDto
                    {
                        Id = result.Data.Id,
                        Description = result.Data.Description,
                        DueDate = result.Data.DueDate,
                        Status = result.Data.Status,
                        AditionalData = result.Data.AditionalData,
                        Active = result.Data.Active,
                        DaysLeft = calculateDaysLeft(result.Data)
                    };


                    var tareas = dto;

                    _logger.LogInformation("Task retrieved successfully.");
                    operationResult = OperationResult.Success("Tareas retrived successfully", tareas);



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

        public async Task<OperationResult> CreateTareaAsync(IEnumerable<TareaAddDto> tareaAddDto)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                if (tareaAddDto == null)
                {
                    return operationResult = OperationResult.Failure("tareaAddDto is null");
                    
                   
                }

                var taskList = tareaAddDto.ToList();
                _logger.LogInformation($"processing tasks: {taskList.Count}");

                var tareasCreadas = new List<TareaAddDto>();
                var tareasFallidas = new List<TareaAddDto>();
                //PROCESAMIENTO DE TAREAS EN COLA.
                var tareasQueue = new Queue<Tarea>(taskList.Select(dto => dto.ToDomainEntityAdd()));

                while (tareasQueue.Count > 0)
                {
                    var tareas = tareasQueue.Dequeue();

                    var result = await _tareaRepository.AddAsync(tareas); 

                    if (result.IsSuccess)
                    {
                        Memoization.ClearAll();
                        _logger.LogInformation($"Task with description '{tareas.Description}' added successfully.");
                        tareasCreadas.Add(result.Data);

                    }
                    else
                    {
                        _logger.LogWarning($"Failed to add task with description '{tareas.Description}': {result.Message}");
                        tareasFallidas.Add(result.Data);

                    }
                }

                if(tareasCreadas.Any())
                {
                    var mensaje = tareasFallidas.Any()
                        ? $"Some tasks were added successfully ({tareasCreadas.Count}), but some failed ({tareasFallidas.Count})."
                        : "All tasks added successfully.";

                    operationResult = OperationResult.Success(mensaje, new
                    {
                        Created = tareasCreadas,
                        Failed = tareasFallidas
                    });
                }
                else
                {
                    operationResult = OperationResult.Failure("No tasks were added successfully.", tareasFallidas);
                }


                    _logger.LogInformation("Successfully added new Tasks");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding a new task: {ex.Message}", ex);
                operationResult = OperationResult.Failure("An error occurred while adding task.");
            }
            return operationResult;
        }

        public async Task<OperationResult> CreateTaskHighPriority(string description)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation($"Adding task with description: {description}");

                if (description == null)
                {
                    operationResult = OperationResult.Failure("Description is null");

                    //return operationResult;

                }

                var tarea = _tareaFactory.CreateTaskHighPriority(description);

                operationResult = await _tareaRepository.AddAsync(tarea);

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

                Memoization.ClearAll();

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

                    Memoization.ClearAll();
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

                string cacheKey = $"status_{status.ToLower()}";

                var tareas = Memoization.GetOrAdd(cacheKey, () =>
                {
                    var result = _tareaRepository.GetByAsync(s => s.Status, status).Result;

                    if (result.IsSuccess && result.Data is not null)
                    {
                        return ((List<Tarea>)result.Data).Select(t => new TareaDto
                        {
                            Id = t.Id,
                            Description = t.Description,
                            DueDate = t.DueDate,
                            Status = t.Status,
                            AditionalData = t.AditionalData,
                            Active = t.Active,
                            DaysLeft = calculateDaysLeft(t)
                        }).ToList();
                    }
                    return new List<TareaDto>();
                });

                if (tareas.Any())
                {
                    operationResult = OperationResult.Success("tasks retrived successfully", tareas);
                }
                else
                {
                    operationResult = OperationResult.Failure("No Tasks found");
                }

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

                string cacheKey = $"duedate_{date.ToString("yyyyMMdd")}";

                var tareas = Memoization.GetOrAdd(cacheKey, () =>
                {
                    var result = _tareaRepository.GetByAsync(s => s.DueDate, date).Result;

                    if (result.IsSuccess && result.Data is not null)
                    {
                        return ((List<Tarea>)result.Data).Select(t => new TareaDto
                        {
                            Id = t.Id,
                            Description = t.Description,
                            DueDate = t.DueDate,
                            Status = t.Status,
                            AditionalData = t.AditionalData,
                            Active = t.Active,
                            DaysLeft = calculateDaysLeft(t)
                        }).ToList();
                    }
                    return new List<TareaDto>();
                });

                if (tareas.Any())
                {
                    operationResult = OperationResult.Success("Tasks retrieved successfully (from cache if repeated)", tareas);
                }
                else
                {
                    operationResult = OperationResult.Failure("No Tasks found");
                }
            



                //var result = await _tareaRepository.GetByAsync(s => s.DueDate, date);

            //if (result.IsSuccess && result.Data is not null)
            //{
            //    var dto = ((List<Tarea>)result.Data).Select(t => new TareaDto
            //    {
            //        Id = t.Id,
            //        Description = t.Description,
            //        DueDate = t.DueDate,
            //        Status = t.Status,
            //        AditionalData = t.AditionalData,
            //        Active = t.Active,
            //        DaysLeft = calculateDaysLeft(t)
            //    });


            //    var tareas = dto.ToList();

            //    operationResult = OperationResult.Success("tasks retrived successfully", tareas);


            //}
            //else
            //{
            //    operationResult = OperationResult.Failure("No Tasks found");

            //}
            //_logger.LogInformation("Successfully retrieved tasks");

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
