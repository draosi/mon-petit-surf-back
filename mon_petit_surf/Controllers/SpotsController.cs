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
        private readonly MonPetitSurfContext _context;
        protected MonPetitSurfService _monPetitSurfService {  get; set; }

        public SpotsController(MonPetitSurfContext context)
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

        [HttpGet("getUtilities")]
        public async Task<ActionResult<List<Utilities>>> getUtilities()
            => await _monPetitSurfService.getUtilities();

        [Authorize]
        [HttpPost("{spotId}/utility/{utilityId}")]
        public async Task<IActionResult> postUtility([FromBody] SpotsGetUtilities model)
        {
            try
            {
                var spot = await _monPetitSurfService.getSpotById(model.SpotId);
                var utility = await _monPetitSurfService.getUtilityById(model.UtilityId);

                if (spot != null && utility != null)
                {
                    _monPetitSurfService.postUtility(model);
                    return Ok("Equipement ajouté au spot");
                } else
                {
                    if (spot == null)
                    {
                        return BadRequest("Spot introuable");
                    } else
                    {
                        return BadRequest("Equipement introuvable");
                    }
                }
            } catch (Exception ex)
            {
                return BadRequest($"une erreur s'est produite : {ex.Message}");
            }
        }
    }
}
