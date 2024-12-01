using Microsoft.AspNetCore.Mvc;
using MobileNotMobileSecondAttempt.Models;
using MobileNotMobileSecondAttempt.Data;
using Microsoft.EntityFrameworkCore;

public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // GET: User/Index
    public async Task<IActionResult> Index()
    {
        var users = await _context.Vartotojai.ToListAsync();
        return View(users);
    }

    // GET: User/Location/{macAddress}
    public async Task<IActionResult> Location(string macAddress)
    {
        var user = await _context.Vartotojai
            .Where(u => u.Mac == macAddress)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound();
        }

        // Here, you would call the method to get the user's measurement location.
        // Assuming you already have a method to calculate the location:
        var location = await GetUserLocation(macAddress);

        return View(location);  // Show the location on a different view
    }

    // Add the method for calculating the user location based on their signal strength
    private async Task<Measurement> GetUserLocation(string macAddress)
    {
        var userSignals = await _context.Vartotojai
            .Where(u => u.Mac == macAddress)
            .ToListAsync();

        // Retrieve measurements and strengths, and find the nearest point as described earlier
        var measurements = await _context.Measurements.ToListAsync();
        var strengths = await _context.Stiprumai.ToListAsync();

        var nearestMeasurement = FindNearestPoint(userSignals, measurements, strengths);

        return nearestMeasurement;
    }

    private Measurement FindNearestPoint(List<User> userSignals, List<Measurement> measurements, List<Strength> strengths)
    {
        Measurement nearestMeasurement = null;
        double minDistance = double.MaxValue;

        foreach (var measurement in measurements)
        {
            var measurementStrengths = strengths.Where(s => s.Matavimas == measurement.Matavimas).ToList();
            double distance = CalculateDistance(userSignals, measurementStrengths);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestMeasurement = measurement;
            }
        }

        return nearestMeasurement;
    }

    private double CalculateDistance(List<User> userSignals, List<Strength> measurementStrengths)
    {
        double distance = 0;

        foreach (var userSignal in userSignals)
        {
            var matchingMeasurementStrength = measurementStrengths
                .FirstOrDefault(s => s.Sensorius == userSignal.Sensorius);

            if (matchingMeasurementStrength != null)
            {
                distance += Math.Pow(userSignal.Stiprumas - matchingMeasurementStrength.Stiprumas, 2);
            }
        }

        return Math.Sqrt(distance);  // Euclidean distance
    }
}
