using Draughts.Core;
using Microsoft.AspNetCore.Mvc;

namespace Draughts.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class DraughtsController : ControllerBase
    {
        [HttpGet("board")]
        public IActionResult Get()
        {
            return Ok(new Draughtboard());
        }
    }
}