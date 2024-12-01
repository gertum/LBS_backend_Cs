namespace MobileNotMobileSecondAttempt.Services
{
    using MobileNotMobileSecondAttempt.Data;
    using MobileNotMobileSecondAttempt.Models;
    using Microsoft.EntityFrameworkCore;

    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            if (_context == null)
                throw new InvalidOperationException("Database context is not initialized.");

            return await _context.Vartotojai.ToListAsync();
        }

        public async Task<List<User>> GetUsersByMacAsync(string macAddress)
        {
            return await _context.Vartotojai
                .Where(u => u.Mac == macAddress)
                .ToListAsync();
        }

        public async Task<(Measurement ClosestGridPoint, double Distance)> GetEstimatedLocationAsync(List<(string Sensor, int SignalStrength)> userSignalStrengths)
        {
            var allStrengths = await _context.Stiprumai.ToListAsync();

            // Group strengths by Matavimas
            var M = allStrengths
                .GroupBy(strength => strength.Matavimas)
                .Select(group => new
                {
                    Matavimas = group.Key,
                    Strengths = new Dictionary<string, int?>
                    {
                { "wiliboxas1", group.FirstOrDefault(s => s.Sensorius == "wiliboxas1")?.Stiprumas }, //i=0
                { "wiliboxas2", group.FirstOrDefault(s => s.Sensorius == "wiliboxas2")?.Stiprumas }, //i=1
                { "wiliboxas3", group.FirstOrDefault(s => s.Sensorius == "wiliboxas3")?.Stiprumas }  //i=2
                    }
                });

            double Dj = int.MaxValue;
            int closestGridPointId=-1;
            // Calculate distances for each Mj
            foreach (var Mj in M)
            {
                double distance = ComputeDistance(userSignalStrengths, Mj.Strengths);
                Console.WriteLine($"Matavimas: {Mj.Matavimas}, Distance: {distance}");
                if(distance < Dj )
                {
                    Dj = distance;
                    closestGridPointId = Mj.Matavimas;
                }

            }
            // Fetch the closest measurement from the database
            var closestMeasurement = await _context.Measurements.FirstOrDefaultAsync(m => m.Matavimas == closestGridPointId);


            // Check if closestMeasurement is null, then throw an exception or return the tuple
            if (closestMeasurement == null)
            {
                throw new InvalidOperationException("No matching measurement found.");
            }

            // Return the closest measurement and the calculated distance
            return (closestMeasurement, Dj);
        }

        private double ComputeDistance(List<(string Sensor, int SignalStrength)> userSignalStrengths, Dictionary<string, int?> strengths)
        {
            return Math.Sqrt(
                userSignalStrengths
                    .Where(user => strengths.ContainsKey(user.Sensor) && strengths[user.Sensor].HasValue)
                    .Sum(user =>
                    {
                        double difference = user.SignalStrength - strengths[user.Sensor].Value;
                        return Math.Pow(difference, 2);
                    })
            );
        }


        public async Task<Dictionary<int, List<GridPoint>>> GetMeasurementGridAsync()
        {
            var measurements = await _context.Measurements
                .Select(m => new { m.X, m.Y, m.Matavimas })
                .ToListAsync();

            return measurements
                .GroupBy(m => m.Y)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(m => new GridPoint { X = m.X, Matavimas = m.Matavimas }).ToList()
                );
        }



        public async Task AddUserMeasurementsAsync(string mac, int signalStrength1, int signalStrength2, int signalStrength3)
        {
            // Check and update or insert the first signal measurement
            var user1 = await _context.Vartotojai
                .FirstOrDefaultAsync(u => u.Mac == mac && u.Sensorius == "wiliboxas1");

            if (user1 != null)
            {
                user1.Stiprumas = signalStrength1; // Update the existing record
            }
            else
            {
                user1 = new User
                {
                    Mac = mac,
                    Sensorius = "wiliboxas1",
                    Stiprumas = signalStrength1
                };
                _context.Vartotojai.Add(user1); // Add a new record
            }

            // Check and update or insert the second signal measurement
            var user2 = await _context.Vartotojai
                .FirstOrDefaultAsync(u => u.Mac == mac && u.Sensorius == "wiliboxas2");

            if (user2 != null)
            {
                user2.Stiprumas = signalStrength2; // Update the existing record
            }
            else
            {
                user2 = new User
                {
                    Mac = mac,
                    Sensorius = "wiliboxas2",
                    Stiprumas = signalStrength2
                };
                _context.Vartotojai.Add(user2); // Add a new record
            }

            // Check and update or insert the third signal measurement
            var user3 = await _context.Vartotojai
                .FirstOrDefaultAsync(u => u.Mac == mac && u.Sensorius == "wiliboxas3");

            if (user3 != null)
            {
                user3.Stiprumas = signalStrength3; // Update the existing record
            }
            else
            {
                user3 = new User
                {
                    Mac = mac,
                    Sensorius = "wiliboxas3",
                    Stiprumas = signalStrength3
                };
                _context.Vartotojai.Add(user3); // Add a new record
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }


        public async Task<LocationResultViewModel> GetLocationForUserAsync(string macAddress)
        {
            var users = await GetUsersByMacAsync(macAddress);

    if (users.Count != 3)
    {
        throw new InvalidOperationException($"Expected 3 users for MAC address {macAddress}, but found {users.Count}.");
    }

    var location = await GetEstimatedLocationAsync(users
        .Select(user => (user.Sensorius, user.Stiprumas))
        .ToList());
            var grid = await GetMeasurementGridAsync();

            return new LocationResultViewModel
            {
                EstimatedLocation = location.ClosestGridPoint,
                EuclideanDistance = location.Distance,
                MeasurementGrid = grid
            };
        }

    }
}
