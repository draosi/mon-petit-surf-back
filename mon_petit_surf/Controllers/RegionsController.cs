using Microsoft.AspNetCore.Mvc;
using MonPetitSurf.Services;

namespace MonPetitSurf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        protected RegionsService _regionsService { get; set; }
        public RegionsController(RegionsService regionsService)
        {
        _regionsService = regionsService;
        }

        [HttpGet("getRegions")]
        public async Task<ActionResult<IEnumerable<string>>> getRegions()
        => await _regionsService.getRegions();

    }
}
