namespace EventEase.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public required string Description { get; set; }

        public Event(string description)
        {
            Description = description;
        }
    }
}