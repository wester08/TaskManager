

namespace TaskManager.Application.Interfaces.Services
{
    public interface ITareaNotifier
    {
        Task NotifyTaskCreatedAsync(object tareaDto);
    }
}
