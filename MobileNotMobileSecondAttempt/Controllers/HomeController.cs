using Microsoft.AspNetCore.Mvc;

namespace MobileNotMobileSecondAttempt.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GridView")]
        public IActionResult GridView()
        {
            // This action serves the Grid.cshtml view
            return View("Grid");
        }

    }
}
