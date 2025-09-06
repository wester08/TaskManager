using Microsoft.AspNetCore.SignalR;

namespace TaskManager.Api.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMsj(string msj)
            => await Clients.All.SendAsync("receiveNotification", msj);
    }
}
