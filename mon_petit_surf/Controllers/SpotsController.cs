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
        protected MonPetitSurfService _monPetitSurfService { get; set; }

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
            var result =  _monPetitSurfService.getSpotById(id);
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
        [HttpGet("{spotId}/utilities")]
        public async Task<IActionResult> getSpotUtilities(int spotId)
        {
            try
            {
                var utilities = _monPetitSurfService.getSpotUtilities(spotId);
                return Ok(utilities);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("{spotId}/utility/{utilityId}")]
        public async Task<IActionResult> postUtility([FromBody] SpotsGetUtilities model)
        {
            try
            {
                var spot =  _monPetitSurfService.getSpotById(model.SpotId);
                var utility =  _monPetitSurfService.getUtilityById(model.UtilityId);

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

        [Authorize]
        [HttpDelete("{spotId}/utility/{utilityId}")]
        public async Task<IActionResult> deleteUtility(int spotId, int utilityId)
        {
            try
            {
                 _monPetitSurfService.deleteUtility(spotId, utilityId);
                return Ok("Equipement supprimé avec succès");
            } catch (Exception ex)
            {
                return BadRequest($"Une erreur s'est produite : {ex.Message}");
            }
        }
    }
}
