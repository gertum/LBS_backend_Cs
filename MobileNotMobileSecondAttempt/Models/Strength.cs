namespace MobileNotMobileSecondAttempt.Models
{

        public class Strength
        {
            public int Id { get; set; } // Primary Key

            public int Matavimas { get; set; } // Foreign Key (likely)

            public string Sensorius { get; set; } // Sensor name

            public int Stiprumas { get; set; } // Signal strength

            //// Optional: Navigation property if there's a relationship with Measurement
            //public Measurement Measurement { get; set; }
        }
    
}
