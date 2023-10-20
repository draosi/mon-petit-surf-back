using Microsoft.AspNetCore.Mvc;

namespace MonPetitSurf.Controllers
{
    public class MonPetitSurfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
