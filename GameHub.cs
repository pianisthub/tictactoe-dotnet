using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MyAspNetCoreApp.Hubs
{
    public class GameHub : Hub
    {
        public async Task NotifyMove(string gameId, object gameState)
        {

            await Clients.Group(gameId).SendAsync("ReceiveGameUpdate", gameState);
        }

        public async Task GameReset(string gameId)
        {
            Console.WriteLine($"  Game {gameId} is resetting for all players.");
            await Clients.Group(gameId).SendAsync("GameReset");
        }


        public async Task JoinGameGroup(string gameId)
        {

            if (string.IsNullOrWhiteSpace(gameId))
            {
                Console.WriteLine("  Error: gameId is null or empty!");
                return;
            }
            try
            {
                Console.WriteLine($"  Player {Context.ConnectionId} joining game {gameId}");
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error in JoinGameGroup: {ex.Message}");
                throw; // Let the error bubble up to frontend
            }
        }
    }
}
