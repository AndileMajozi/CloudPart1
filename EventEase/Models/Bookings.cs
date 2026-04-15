namespace EventEase.Models
{
    public class Booking
    {
        public string BookingID { get; set; } = Guid.NewGuid().ToString(); 
        public int VenueID { get; set; }
        public Venue? Venue { get; set; } 
        public int EventID { get; set; }
        public Event? Event { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
