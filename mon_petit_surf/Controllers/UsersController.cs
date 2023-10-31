using Microsoft.AspNetCore.Mvc;
using MonPetiSurf.Context;
using MonPetitSurf.Services;

namespace MonPetitSurf.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly MonPetitSurfContext _context;
        protected MonPetitSurfService _monPetitSurfService { get; set; }

        public UsersController(MonPetitSurfContext context)
        {
            _context = context;
            _monPetitSurfService = new MonPetitSurfService(_context);
        }
    }
}
