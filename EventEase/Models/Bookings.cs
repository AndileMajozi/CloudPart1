namespace EventEase.Models
{
    public class Booking
    {
        public string BookingID { get; set; } = Guid.NewGuid().ToString(); // Makes a unique random ID
        public int VenueID { get; set; }
        public Venue? Venue { get; set; } // Link to Venue
        public int EventID { get; set; }
        public Event? Event { get; set; } // Link to Event
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}