using System;
using System.Threading.Tasks;
using Draughts.Server.Services;
using Draughts.Shared.Models;
using Draughts.Shared.Models.Board;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> MakeMove(Guid guid, [FromBody] AvailableMove move)
        {
            move = await _games.MakeMove(guid, move);

            if (move is null)
            {
                return BadRequest("Invalid move!");
            }
            
            return Ok(move);
        }
    }
}