using Microsoft.AspNetCore.Mvc; 
using MobileNotMobileSecondAttempt.Models;
using MobileNotMobileSecondAttempt.Services;

namespace MobileNotMobileSecondAttempt.Controllers
{
    public class NewMacUserController : Controller
    {
        private readonly UserService _locationService;

        public NewMacUserController(UserService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LocationRequestNewMac request)
        {

            //return View(viewModel);

            // Insert the new user measurements
            await _locationService.AddUserMeasurementsAsync(request.Mac, request.SignalStrength1, request.SignalStrength2, request.SignalStrength3);

            // Pass the MAC address to the Location view
            var locationResult = await _locationService.GetLocationForUserAsync(request.Mac);

            if (locationResult == null)
            {
                return NotFound();
            }

            // Load the Location.cshtml view from the Shared folder
            return View("~/Views/Shared/Location.cshtml", locationResult);
        }
    }
}
