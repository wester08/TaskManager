

using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces.Factories
{
    public interface ITareaFactory
    {
       Tarea CreateTaskHighPriority(string description);
    }
}
