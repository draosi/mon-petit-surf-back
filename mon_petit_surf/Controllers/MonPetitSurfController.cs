using Microsoft.AspNetCore.Mvc;
using MonPetiSurf.Context;
using MonPetitSurf.Models;
using MonPetitSurf.Services;

namespace MonPetitSurf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonPetitSurfController : ControllerBase
    {
        private readonly MonPetitSurfContext _context;
        protected MonPetitSurfService _monPetitSurfService {  get; set; }

        public MonPetitSurfController(MonPetitSurfContext context)
        {
            _context = context;
            _monPetitSurfService = new MonPetitSurfService(_context);
        }

        [HttpGet]
        [Route("getSpots")]
        public async Task<ActionResult<List<Spots>>> getSpots()
            => await _monPetitSurfService.getSpots();

        [HttpGet("getSpot/{id}")]
        public async Task<ActionResult<Spots>> getSpotById(int id)
        {
            var result = await _monPetitSurfService.getSpotById(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpGet("getRegions")]
        public async Task<ActionResult<IEnumerable<string>>> getRegions()
            => await _monPetitSurfService.getRegions();
    }
}
