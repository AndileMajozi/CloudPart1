using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Booking
    {
        [Key]
        [Display(Name = "Booking ID")]
        public string BookingID { get; set; } = Guid.NewGuid().ToString("N")[..10].ToUpper();

        [Required]
        [Display(Name = "Venue")]
        public int VenueID { get; set; }
        public Venue? Venue { get; set; }

        [Required]
        [Display(Name = "Event")]
        public int EventID { get; set; }
        public Event? Event { get; set; }

        [Required]
        [Display(Name = "Start Date and Time")]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(1);

        [Required]
        [Display(Name = "End Date and Time")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1).AddHours(2);
    }
}
