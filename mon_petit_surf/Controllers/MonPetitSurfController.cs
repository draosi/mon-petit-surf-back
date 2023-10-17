using Microsoft.AspNetCore.Mvc;

namespace mon_petit_surf.Controllers
{
    public class MonPetitSurfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
