using Microsoft.AspNetCore.SignalR;
using TaskManager.Application.Interfaces.Services;
using TaskManager.Api.Hubs;

namespace TaskManager.Api.Notifications
{
    public class SignalRTareaNotifier : ITareaNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRTareaNotifier(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task NotifyTaskCreatedAsync(object tareaDto)
        {
            await _hubContext.Clients.All.SendAsync("TaskCreated", tareaDto);
        }
    }
}
