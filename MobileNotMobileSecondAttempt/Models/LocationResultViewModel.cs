namespace MobileNotMobileSecondAttempt.Models
{
    public class LocationResultViewModel
    {
        public Measurement EstimatedLocation { get; set; }
        public double EuclideanDistance { get; set; }
        public Dictionary<int, List<GridPoint>> MeasurementGrid { get; set; }
    }
}
