using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonPetitSurf.Models;
using MonPetitSurf.Services;

namespace MonPetitSurf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotsController : ControllerBase
    {
        protected SpotsService _spotsService { get; set; }

        public SpotsController(SpotsService spotsService)
        {
            _spotsService = spotsService;
        }

        [HttpGet]
        [Route("getSpots")]
        public async Task<ActionResult<List<Spots>>> getSpots()
            => await _spotsService.getSpots();

        [HttpGet("getSpot/{id}")]
        public async Task<ActionResult<Spots>> getSpotById(int id)
        {
            var result =  _spotsService.getSpotById(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
    }
}
