
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces.Factories
{
    public class TareaFactory : ITareaFactory
    {
        public Tarea CreateTaskHighPriority(string description)
        {
            return new Tarea
            {

                Description = description,
                DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                Status = "Pendiente",
                AditionalData = "High Priority",
                Active = true
            };
        }
    }
}
