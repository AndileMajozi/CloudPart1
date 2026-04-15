using EventEase; // This tells it where to find Venue, Event, and Bookings
using EventEase.Models;
using Microsoft.EntityFrameworkCore; // THIS IS LIKELY MISSING

namespace EventEase.Data
{
    public class ApplicationDbContext : DbContext
    {
        // This constructor MUST look like this
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}