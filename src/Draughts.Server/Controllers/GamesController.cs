using System;
using System.Linq;
using System.Threading.Tasks;
using Draughts.Server.Models;
using Draughts.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Draughts.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GamesService _games;

        public GamesController(GamesService games)
        {
            _games = games;
        }

        [HttpGet]
        public IActionResult ListGames()
        {
            return Ok(_games.Games.Select(x => new ListGameDto {Id = x.Id, Name = x.Name, IsPublic = x.IsPublic}));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto dto)
        {
            var game = await _games.CreateGame(dto.Name, dto.MakePublic);

            if (game is null)
            {
                return BadRequest();
            }
            
            return Ok(game);
        }
        
        [HttpDelete("{guid:guid}")]
        public async Task<IActionResult> DeleteGame(Guid guid)
        {
            var result = await _games.DeleteGame(guid);

            if (!result)
            {
                return NotFound();
            }
            
            return Ok();
        }
        
        [HttpGet("{guid:guid}")]
        public IActionResult GetGame(Guid guid)
        {
            var game = _games.GetGame(guid);

            if (game is null)
            {
                return NotFound();
            }

            return Ok(game);
        }
    }
}