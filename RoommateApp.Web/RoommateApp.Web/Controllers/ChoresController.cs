using Microsoft.AspNetCore.Mvc;
using RoommateApp.Bll.DTOs;
using RoommateApp.Bll.Services;

namespace RoommateApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChoresController : ControllerBase
    {
        private readonly ChoreService _choreService;

        public ChoresController(ChoreService choreService)
        {
            _choreService = choreService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChoreDto>>> GetChores()
        {
            var chores = await _choreService.GetChoresAsync();
            return Ok(chores);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<string>>> GetCategories()
        {
            var categories = await _choreService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<ChoreDto>> CreateChore([FromBody] CreateChoreDto dto)
        {
            var chore = await _choreService.CreateChoreAsync(dto);
            return Ok(chore);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChore(int id, [FromBody] UpdateChoreDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var success = await _choreService.UpdateChoreStatusAsync(dto);
            if (!success) return NotFound();

            return Ok();
        }
    }
}