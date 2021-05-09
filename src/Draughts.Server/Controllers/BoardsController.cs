using System;
using System.Threading.Tasks;
using Draughts.Server.Hubs;
using Draughts.Server.Models;
using Draughts.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Draughts.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly GamesService _games;

        public BoardsController(GamesService games)
        {
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

        [HttpPost("{guid:guid}/moves")]
        public async Task<IActionResult> MakeMove(Guid guid, [FromBody] MakeMoveDto dto)
        {
            var (source, destination) = dto;
            var move = await _games.MakeMove(guid, source, destination);

            if (move is null)
            {
                return BadRequest("Invalid move!");
            }
            
            return Ok(move);
        }
    }
}