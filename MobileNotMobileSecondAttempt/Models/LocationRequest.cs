namespace MobileNotMobileSecondAttempt.Models
{
    public class LocationRequest
    {
        public string Mac { get; set; }
        public List<int> SignalStrengths { get; set; } // Signal strengths from different sensors
    }

}
