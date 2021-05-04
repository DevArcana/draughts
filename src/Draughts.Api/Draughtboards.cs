using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Draughts.Api.Hubs;
using Draughts.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Draughts.Api
{
    public class Draughtboards
    {
        private readonly IServiceProvider _services;
        private IDictionary<string, Draughtboard> _draughtboards;

        public Draughtboards(IServiceProvider services)
        {
            _services = services;
            _draughtboards = new Dictionary<string, Draughtboard>();
        }

        public async Task<Draughtboard> CreateDraughtboard(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            
            if (_draughtboards.ContainsKey(name))
            {
                return null;
            }

            var draughtboard = new Draughtboard();
            _draughtboards[name] = draughtboard;

            using var scope = _services.CreateScope();
            var hub = scope.ServiceProvider.GetService<IHubContext<DraughtsHub>>();
            await hub.Clients.Group("lobby").SendAsync("BoardCreated", name);

            return draughtboard;
        }

        public IEnumerable<string> AvailableDraughtboards => _draughtboards.Keys;

        public Draughtboard GetDraughtboard(string name)
        {
            return _draughtboards.TryGetValue(name, out var board) ? board : null;
        }
    }
}