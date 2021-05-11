using System;
using System.Threading.Tasks;
using Draughts.Server.Services;
using Draughts.Shared.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Draughts.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/{guid:guid}")]
    public class BoardsController : ControllerBase
    {
        private readonly GamesService _games;

        public BoardsController(GamesService games)
        {
            _games = games;
        }

        [HttpGet]
        public IActionResult GetBoard(Guid guid)
        {
            var board = _games.GetBoard(guid);

            if (board is null)
            {
                return NotFound();
            }

            return Ok(board);
        }

        [HttpPost("moves")]
        public async Task<IActionResult> MakeMove(Guid guid, [FromBody] MakeMoveDto dto)
        {
            var move = await _games.MakeMove(guid, dto.Identifier);

            if (move is null)
            {
                return BadRequest("Invalid move!");
            }
            
            return Ok(move);
        }
    }
}