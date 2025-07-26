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

        public override async Task<OperationResult> AddAsync(Tarea entity)
        {

                
                if (entity == null)
                {
                    return OperationResult.Failure("Entity cannot be null.");

                }

                if (string.IsNullOrWhiteSpace(entity.Description))
                {
                    return OperationResult.Failure("Entity description cannot be null or empty.");
                }

                var today = DateOnly.FromDateTime(DateTime.Now);

                if (entity.DueDate <= today)
                {
                    return OperationResult.Failure("You must specify a date greater than today");
                }

                var existingEntity = await ExistsAsync(t => t.Description == entity.Description);
                if (existingEntity)
                {
                    return OperationResult.Failure("An entity with the same description already exists.");
                }

               
                
            

                return await base.AddAsync(entity);


        }
        public override async Task<OperationResult> UpdateAsync(Tarea entity)
        {
            if (entity == null)
            {
                return OperationResult.Failure("Entity cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(entity.Description))
            {
                return OperationResult.Failure("Entity description cannot be null or empty.");
            }
            var existingEntity = await ExistsAsync(c => c.Id != entity.Id && c.Description == entity.Description);
            if (existingEntity)
            {
                return OperationResult.Failure("An entity with the same description already exists.");
            }

            var today = DateOnly.FromDateTime(DateTime.Now);

            if (entity.DueDate <= today)
            {
                return OperationResult.Failure("You must specify a date greater than today");
            }
            _context.Tareas.Update(entity);
            await _context.SaveChangesAsync();
            return OperationResult.Success("Entity updated successfully.", entity);
        }

      

    }

}
