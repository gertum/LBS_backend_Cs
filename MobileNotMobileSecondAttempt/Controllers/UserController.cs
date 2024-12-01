using Microsoft.AspNetCore.Mvc;
using MobileNotMobileSecondAttempt.Models;
using MobileNotMobileSecondAttempt.Services;

public class UserController : Controller
{

    private readonly UserService _locationService;

    public UserController(UserService locationService)
    {
        _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
    }

    public async Task<IActionResult> Index()
    {
        // Fetch all user data from the database
        var users = await _locationService.GetAllUsersAsync();

        // Group data by MAC address
        var groupedData = users
            .GroupBy(user => user.Mac)
            .Select(group => new
            {
                Mac = group.Key,
                Wiliboxas1 = group.FirstOrDefault(u => u.Sensorius == "wiliboxas1")?.Stiprumas ?? 404,
                Wiliboxas2 = group.FirstOrDefault(u => u.Sensorius == "wiliboxas2")?.Stiprumas ?? 404,
                Wiliboxas3 = group.FirstOrDefault(u => u.Sensorius == "wiliboxas3")?.Stiprumas ?? 404
            })
            .ToList();

        // Pass the grouped data to the view
        return View(groupedData);
    }

   public async Task<IActionResult> Location(string macAddress)
{
    var locationResult = await _locationService.GetLocationForUserAsync(macAddress);

    if (locationResult == null)
    {
        return NotFound();
    }

        return View("~/Views/Shared/Location.cshtml", locationResult);
    }

}
