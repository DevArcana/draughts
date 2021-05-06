﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Draughts.Server.Hubs;
using Draughts.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace Draughts.Server.Services
{
    public class GamesService
    {
        private readonly List<Game> _games = new();
        private readonly Dictionary<Guid, Draughtboard> _boards = new();

        private readonly IHubContext<GamesHub> _hub;

        public GamesService(IHubContext<GamesHub> hub)
        {
            _hub = hub;
        }

        public async Task<Game> CreateGame(string name, bool makePublic)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Game's name can't be empty!");
            }
            
            var game = new Game(Guid.NewGuid(), name, makePublic);

            _games.Add(game);
            _boards[game.Id] = new Draughtboard();

            if (makePublic)
            {
                await _hub.Clients.All.SendAsync("GameCreated", game.Id, game.Name);
            }
            
            return game;
        }

        public IEnumerable<Game> Games => _games.Where(x => x.IsPublic);

        public async Task<bool> DeleteGame(Guid id)
        {
            var game = _games.FirstOrDefault(x => x.Id == id);

            if (game is null)
            {
                return false;
            }

            var deleted = _games.Remove(game);
            
            if (game.IsPublic && deleted)
            {
                await _hub.Clients.All.SendAsync("GameDeleted", game.Id);
                _boards.Remove(game.Id);
            }

            return deleted;
        }

        public Draughtboard GetBoard(Guid id)
        {
            return _boards.TryGetValue(id, out var board) ? board : null;
        }

        public Game GetGame(Guid id)
        {
            return _games.FirstOrDefault(x => x.Id == id);
        }
    }
}