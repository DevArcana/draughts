using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Draughts.Api.Hubs
{
    public class DraughtsHub : Hub
    {
        public async Task JoinBoard(string board)
        {
            var connection = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(connection, "lobby");
            await Groups.AddToGroupAsync(connection, board);
        }
        
        public async Task LeaveBoard(string board)
        {
            var connection = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(connection, board);
            await Groups.AddToGroupAsync(connection, "lobby");
        }

        public override async Task OnConnectedAsync()
        {
            var connection = Context.ConnectionId;
            await Groups.AddToGroupAsync(connection, "lobby");
        }
    }
}