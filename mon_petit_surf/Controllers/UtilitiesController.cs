using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonPetitSurf.Models;
using MonPetitSurf.Services;

namespace MonPetitSurf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilitiesController : ControllerBase
    {
        protected UtilitiesService _utilitiesService { get; set; }
        protected SpotsService _spotsService { get; set; }

        public UtilitiesController(UtilitiesService utilitiesService, SpotsService spotsService)
        {
            _utilitiesService = utilitiesService;
            _spotsService = spotsService;
        }


        [HttpGet("getUtilities")]
        public async Task<ActionResult<List<Utilities>>> getUtilities()
        => await _utilitiesService.getUtilities();

        [Authorize]
        [HttpGet("{spotId}/utilities")]
        public async Task<IActionResult> getSpotUtilities(int spotId)
        {
            try
            {
                var utilities = _utilitiesService.getSpotUtilities(spotId);
                return Ok(utilities);
            }
            catch (Exception ex)
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
                var spot = _spotsService.getSpotById(model.SpotId);
                var utility = _utilitiesService.getUtilityById(model.UtilityId);

                if (spot != null && utility != null)
                {
                    await _utilitiesService.postUtility(spot, utility);
                    return Ok("Equipement ajouté au spot");
                }
                else
                {
                    if (spot == null)
                    {
                        return BadRequest("Spot introuable");
                    }
                    else
                    {
                        return BadRequest("Equipement introuvable");
                    }
                }
            }
            catch (Exception ex)
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
                _utilitiesService.deleteUtility(spotId, utilityId);
                return Ok("Equipement supprimé avec succès");
            }
            catch (Exception ex)
            {
                return BadRequest($"Une erreur s'est produite : {ex.Message}");
            }
        }
    }
}
