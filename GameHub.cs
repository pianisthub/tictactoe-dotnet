using Microsoft.AspNetCore.SignalR;

namespace MyAspNetCoreApp.Hubs
{
    public class GameHub : Hub
    {
        public async Task NotifyMove(string gameId, object gameState)
        {
             
            await Clients.Group(gameId).SendAsync("ReceiveGameUpdate", gameState);
        }

        public async Task JoinGameGroup(string gameId)
        {
            // Add the client to a SignalR group for the game
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }
    }
}
