namespace MobileNotMobileSecondAttempt.Models
{
    public class User
    {
        public int Id { get; set; } // Primary Key

        public string Mac { get; set; } // MAC address

        public string Sensorius { get; set; } // Sensor name

        public int Stiprumas { get; set; } // Signal strength
    }
}
