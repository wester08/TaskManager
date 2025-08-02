
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Interfaces.Respositories.Task;
using TaskManager.Domain.Base;
using TaskManager.Domain.Entities;
using TaskManager.Persistence.Base;
using TaskManager.Persistence.Context;

namespace TaskManager.Persistence.Repositories
{
    public sealed class TareaRepository : BaseRepository<Tarea>, ITareaRepository
    {

        private readonly TaskManagerContext _context;


        public TareaRepository(TaskManagerContext context) : base(context)
        {
            _context = context;

        }

        //delegate desription
        delegate OperationResult validateTask(Tarea entity);

        validateTask validate = entity =>
        {
            if (entity == null)
                return OperationResult.Failure("Entity cannot be null.");

            if (string.IsNullOrWhiteSpace(entity.Description))
                return OperationResult.Failure("Entity description cannot be null or empty.");

            var today = DateOnly.FromDateTime(DateTime.Now);
            if (entity.DueDate <= today)
                return OperationResult.Failure("You must specify a date greater than today.");

            return OperationResult.Success("Entity success");
        };


        //Action Task
        Action<Tarea> notifyCreation = task
            => Console.WriteLine($"Task with description {task.Description} create.");


        //Func para calculos derivados.
        Func<Tarea, int> calculateDaysLeft = task
             =>
        {
            var DueDateTime = task.DueDate.ToDateTime(TimeOnly.MinValue);
            var days = (DueDateTime - DateTime.Now).Days;

            if (task.Status == "Completado")
            {
                return 0;
            }
            else
            {

            }
            return days;
        };



        public override async Task<OperationResult> AddAsync(Tarea entity)
        {


            var validations = validate(entity);
            if (!validations.IsSuccess)
            {
                return validations;
            }


            var existingEntity = await ExistsAsync(t => t.Description == entity.Description);
            if (existingEntity)
            {
                return OperationResult.Failure("An entity with the same description already exists.");
            }

             

            var result = await base.AddAsync(entity);

            if (result.IsSuccess)
            {

                var dto = new TareaAddDto
                {
                    Description = entity.Description,
                    DueDate = entity.DueDate,
                    Status = entity.Status,
                    AditionalData = entity.AditionalData,
                    Active = entity.Active,
                    DaysLeft = calculateDaysLeft(entity)
                };

                notifyCreation(entity);

                return OperationResult.Success("Task create susccessfuly", dto);
            }

            return result;



        }
        public override async Task<OperationResult> UpdateAsync(Tarea entity)
        {

            var validations = validate(entity);
            if (!validations.IsSuccess)
            {
                return validations;
            }
 

            var existingEntity = await ExistsAsync(c => c.Id != entity.Id && c.Description == entity.Description);
            if (existingEntity)
            {
                return OperationResult.Failure("An entity with the same description already exists.");
            }

            _context.Tareas.Update(entity);
            await _context.SaveChangesAsync();
            return OperationResult.Success("Entity updated successfully.", entity);


        }



    }

}
