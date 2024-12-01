using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileNotMobileSecondAttempt.Data;
using MobileNotMobileSecondAttempt.Models;

namespace MobileNotMobileSecondAttempt.Controllers
{
    public class LocationController : Controller
    {
        private readonly AppDbContext _context;

        public LocationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult EstimateLocation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EstimateLocation(LocationRequest request)
        {
            // Validate the MAC address and signal strengths
            if (string.IsNullOrEmpty(request.Mac) || request.SignalStrengths == null || request.SignalStrengths.Count == 0)
            {
                return BadRequest("Invalid input.");
            }

            // Use the signal strengths and MAC address to find the closest location
            var estimatedLocation = await GetEstimatedLocationAsync(request);

            // Return the estimated location as a response (you could render it in a view or return it as JSON)
            return View("LocationResult", estimatedLocation);
        }

        private async Task<Measurement> GetEstimatedLocationAsync(LocationRequest request)
        {
            // Fetch the user's MAC address signal strengths
            var userSignalStrengths = request.SignalStrengths;

            // Fetch all strength data from the database for comparison
            var allStrengths = await _context.Stiprumai.ToListAsync(); // Fetch without including Measurement

            // Find the closest measurement point using a nearest neighbor algorithm
            Measurement closestMeasurement = null;
            double closestDistance = double.MaxValue;

            foreach (var strength in allStrengths)
            {
                // Compute the Euclidean distance (or any other distance metric)
                double distance = ComputeDistance(userSignalStrengths, strength.Stiprumas);

                // Get the related Measurement object using the Matavimas (foreign key)
                var measurement = await _context.Measurements.FindAsync(strength.Matavimas);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMeasurement = measurement; // Assign the full Measurement object
                }
            }

            return closestMeasurement; // This returns the closest Measurement (location)
        }


        // Simple Euclidean distance calculation between signal strength vectors
        private double ComputeDistance(List<int> signalStrengths, int signalStrength)
        {
            return Math.Sqrt(signalStrengths.Select((s, i) => Math.Pow(s - signalStrength, 2)).Sum());
        }
    }

}
