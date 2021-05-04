using System.Threading.Tasks;
using Draughts.Core;
using Microsoft.AspNetCore.Mvc;

namespace Draughts.Api.Controllers
{
    public class DraughtboardDto
    {
        public string Name { get; set; }
    }
    
    [ApiController]
    [Route("api")]
    public class DraughtsController : ControllerBase
    {
        private readonly Draughtboards _draughtboards;

        public DraughtsController(Draughtboards draughtboards)
        {
            _draughtboards = draughtboards;
        }

        [HttpGet("boards/{name}")]
        public IActionResult GetBoard(string name)
        {
            var board = _draughtboards.GetDraughtboard(name);

            if (board is null)
            {
                return NotFound();
            }
            
            return Ok(board);
        }

        [HttpGet("boards")]
        public IActionResult GetBoards()
        {
            return Ok(_draughtboards.AvailableDraughtboards);
        }
        
        [HttpPost("boards")]
        public async Task<IActionResult> CreateBoard([FromBody] DraughtboardDto dto)
        {
            var board = await _draughtboards.CreateDraughtboard(dto.Name);
            
            if (board is null)
            {
                return BadRequest("invalid or already existing board name!");
            }
            
            return CreatedAtAction(nameof(GetBoard), new {name = dto.Name}, dto.Name);
        }
    }
}