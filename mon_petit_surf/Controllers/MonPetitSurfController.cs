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
    }
}
