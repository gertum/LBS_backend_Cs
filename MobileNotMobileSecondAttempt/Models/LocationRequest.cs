namespace MobileNotMobileSecondAttempt.Models
{
    public class LocationRequest
    {
        public string Mac { get; set; }
        public List<int> SignalStrengths { get; set; } // Signal strengths from different sensors

        // This property is needed for binding the signal strengths string from the form
        public string SignalStrengthString { get; set; }
    }

}
