using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoommateApp.Bll.DTOs;
using RoommateApp.Bll.Services;

namespace RoommateApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentController : ControllerBase
    {
        private readonly ApartmentService _apartmentService;

        public ApartmentController(ApartmentService apartmentService)
        {
            _apartmentService = apartmentService;
        }

        // Szoba lekérése ID alapján
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentDto>> GetApartment(int id)
        {
            var apt = await _apartmentService.GetApartmentByIdAsync(id);
            if (apt == null) return NotFound();
            return Ok(apt);
        }

        [HttpPut]
        public async Task<ActionResult<ApartmentDto>> UpdateApartment([FromBody] ApartmentDto dto)
        {
            var updated = await _apartmentService.UpdateApartmentAsync(dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // ÚJ: Szoba törlése ID alapján
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(int id)
        {
            var success = await _apartmentService.DeleteApartmentAsync(id);
            if (!success) return NotFound("A megadott szoba nem található.");
            return NoContent();
        }
    }
}