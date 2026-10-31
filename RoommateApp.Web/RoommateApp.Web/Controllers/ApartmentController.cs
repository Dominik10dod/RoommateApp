using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<ApartmentDto>> GetApartment()
        {
            var apt = await _apartmentService.GetApartmentAsync();
            return Ok(apt);
        }

        [HttpPut]
        public async Task<ActionResult<ApartmentDto>> UpdateApartment([FromBody] ApartmentDto dto)
        {
            var updated = await _apartmentService.UpdateApartmentAsync(dto);
            return Ok(updated);
        }
    }
}