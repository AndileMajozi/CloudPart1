using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public record Venue([property: Key] int VenueID, [property: Required] string Name, string Location, int Capacity, string ImageUrl)
    {
        public Venue(string imageUrl) : this(default, default, default, default, default)
        {
            ImageUrl = imageUrl;
        }
    }
}