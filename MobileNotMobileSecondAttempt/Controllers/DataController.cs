using Microsoft.AspNetCore.Mvc;
using MobileNotMobileSecondAttempt.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MobileNotMobileSecondAttempt.Models;

namespace MobileNotMobileSecondAttempt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DataController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Data/Measurements
        [HttpGet("matavimai")]
        public async Task<IActionResult> GetMeasurements()
        {
            var measurements = await _context.Measurements.ToListAsync();
            return Ok(measurements);
        }

        // GET: api/Data/Stiprumai
        [HttpGet("stiprumai")]
        public async Task<IActionResult> GetStiprumai()
        {
            var stiprumai = await _context.Stiprumai.ToListAsync();
            return Ok(stiprumai);
        }

        // GET: api/Data/Vartotojai
        [HttpGet("vartotojai")]
        public async Task<IActionResult> GetVartotojai()
        {
            var vartotojai = await _context.Vartotojai.ToListAsync();
            return Ok(vartotojai);
        }

        // GET: api/Data/Grid
        [HttpGet("Grid")]
        public async Task<IActionResult> GetMeasurementGrid()
        {
            var measurements = await _context.Measurements
                .Select(m => new { m.X, m.Y, m.Matavimas })
                .ToListAsync();

            // Group data by Y-coordinate and prepare it with X and IDs
            var grid = measurements
                .GroupBy(m => m.Y)
                .ToDictionary(
                    group => group.Key, // Y-coordinate
                    group => group.Select(m => new { m.X, m.Matavimas }).ToList() // List of X and IDs
                );

            return Ok(grid);
        }



        //[HttpGet("user-location/{macAddress}")]
        //public async Task<IActionResult> GetUserLocation(string macAddress)
        //{
        //    // Retrieve the user's signal strengths from the database
        //    var userSignals = await _context.Vartotojai
        //        .Where(u => u.Mac == macAddress)
        //        .ToListAsync();

        //    if (userSignals == null || !userSignals.Any())
        //    {
        //        return NotFound("No signal strength data found for this user.");
        //    }

        //    // Retrieve all the measurement points and their associated strengths
        //    var measurements = await _context.Measurements.ToListAsync();
        //    var strengths = await _context.Stiprumai.ToListAsync();

        //    // Determine the nearest measurement point based on signal strength
        //    var nearestMeasurement = FindNearestPoint(userSignals, measurements, strengths);

        //    return Ok(new { Location = nearestMeasurement });
        //}

        //private Measurement FindNearestPoint(List<User> userSignals, List<Measurement> measurements, List<Strength> strengths)
        //{
        //    Measurement nearestMeasurement = null;
        //    double minDistance = double.MaxValue;

        //    // Loop through all measurements to compare them with the user's signal strengths
        //    foreach (var measurement in measurements)
        //    {
        //        // Get the signal strengths for the current measurement point
        //        var measurementStrengths = strengths.Where(s => s.Matavimas == measurement.Matavimas).ToList();

        //        // Calculate distance between user signal strengths and measurement point signal strengths
        //        double distance = CalculateDistance(userSignals, measurementStrengths);

        //        // If this measurement point is closer, update the nearest point
        //        if (distance < minDistance)
        //        {
        //            minDistance = distance;
        //            nearestMeasurement = measurement;
        //        }
        //    }

        //    return nearestMeasurement;
        //}

        //private double CalculateDistance(List<User> userSignals, List<Strength> measurementStrengths)
        //{
        //    double distance = 0;

        //    // Iterate through the user's signals and corresponding measurement strengths
        //    foreach (var userSignal in userSignals)
        //    {
        //        var matchingMeasurementStrength = measurementStrengths
        //            .FirstOrDefault(s => s.Sensorius == userSignal.Sensorius);

        //        if (matchingMeasurementStrength != null)
        //        {
        //            // Calculate the difference between the user's signal and the measurement signal
        //            distance += Math.Pow(userSignal.Stiprumas - matchingMeasurementStrength.Stiprumas, 2);
        //        }
        //    }

        //    return Math.Sqrt(distance);  // Euclidean distance formula
        //}
    }
}
