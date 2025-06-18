using GameStore.Meta.SignalR.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GameStore.Meta.SignalR.Hubs
{
    [Authorize(AuthenticationSchemes = "NotificationApiKeyScheme")]
    public class NotificationHub : Hub
    {
        readonly HubConnectionManager<NotificationHub> HubConnectionManager;

        public NotificationHub(HubConnectionManager<NotificationHub> hubConnectionManager)
        {
            HubConnectionManager = hubConnectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            HubConnectionManager.AddConnection(Context.UserIdentifier, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context.UserIdentifier, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
