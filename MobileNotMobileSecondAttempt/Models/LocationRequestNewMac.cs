using System.ComponentModel.DataAnnotations;

namespace MobileNotMobileSecondAttempt.Models
{
    public class LocationRequestNewMac
    {

        [Required(ErrorMessage = "MAC address is required.")]
        [RegularExpression(
            @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$",
            ErrorMessage = "Invalid MAC address format. Example: 00:1A:2B:3C:4D:5E"
        )]
        public string Mac { get; set; }

        [Required(ErrorMessage = "SignalStrength1 is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "SignalStrength1 must be greater than or equal to 0.")]
        public int SignalStrength1 { get; set; }

        [Required(ErrorMessage = "SignalStrength2 is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "SignalStrength2 must be greater than or equal to 0.")]
        public int SignalStrength2 { get; set; }

        [Required(ErrorMessage = "SignalStrength3 is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "SignalStrength3 must be greater than or equal to 0.")]
        public int SignalStrength3 { get; set; }
    }
}

