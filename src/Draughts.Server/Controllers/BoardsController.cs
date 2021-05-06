using System;
using Draughts.Server.Hubs;
using Draughts.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Draughts.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly IHubContext<BoardsHub> _hub;
        private readonly GamesService _games;

        public BoardsController(IHubContext<BoardsHub> hub, GamesService games)
        {
            _hub = hub;
            _games = games;
        }

        [HttpGet("{guid:guid}")]
        public IActionResult GetBoard(Guid guid)
        {
            var board = _games.GetBoard(guid);

            if (board is null)
            {
                return NotFound();
            }

            return Ok(board);
        }
    }
}