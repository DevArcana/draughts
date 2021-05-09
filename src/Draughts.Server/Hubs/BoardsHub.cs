using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Draughts.Server.Hubs
{
    public class BoardsHub : Hub
    {
        private readonly IDictionary<string, string> _groups = new Dictionary<string, string>();
        public async Task WatchBoard(string board)
        {
            var connectionId = Context.ConnectionId;
            
            if (_groups.ContainsKey(connectionId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, _groups[connectionId]);
            }

            _groups[connectionId] = board;
            await Groups.AddToGroupAsync(connectionId, board);
        }
    }
}