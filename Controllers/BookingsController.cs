using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = searchString.Trim().ToLower();
                bookings = bookings.Where(b =>
                    b.BookingID.ToLower().Contains(term) ||
                    (b.Event != null && b.Event.EventName.ToLower().Contains(term)));
            }

            return View(await bookings.OrderBy(b => b.StartDate).ToListAsync());
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null) return NotFound();
            var booking = await _context.Bookings.Include(b => b.Event).Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingID == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        public IActionResult Create()
        {
            LoadDropDowns();
            return View(new Booking());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            ValidateBookingTimes(booking);
            await ValidateDoubleBooking(booking);

            if (!ModelState.IsValid)
            {
                LoadDropDowns(booking.VenueID, booking.EventID);
                return View(booking);
            }

            booking.BookingID = string.IsNullOrWhiteSpace(booking.BookingID)
                ? Guid.NewGuid().ToString("N")[..10].ToUpper()
                : booking.BookingID.ToUpper();

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Booking created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return NotFound();
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            LoadDropDowns(booking.VenueID, booking.EventID);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Booking booking)
        {
            if (id != booking.BookingID) return NotFound();

            ValidateBookingTimes(booking);
            await ValidateDoubleBooking(booking, id);

            if (!ModelState.IsValid)
            {
                LoadDropDowns(booking.VenueID, booking.EventID);
                return View(booking);
            }

            _context.Update(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Booking updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null) return NotFound();
            var booking = await _context.Bookings.Include(b => b.Event).Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingID == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropDowns(int? selectedVenue = null, int? selectedEvent = null)
        {
            ViewData["VenueID"] = new SelectList(_context.Venues.OrderBy(v => v.Name), "VenueID", "Name", selectedVenue);
            ViewData["EventID"] = new SelectList(_context.Events.OrderBy(e => e.EventName), "EventID", "EventName", selectedEvent);
        }

        private void ValidateBookingTimes(Booking booking)
        {
            if (booking.EndDate <= booking.StartDate)
            {
                ModelState.AddModelError(string.Empty, "The booking end date and time must be after the start date and time.");
            }
        }

        private async Task ValidateDoubleBooking(Booking booking, string? currentBookingId = null)
        {
            var clash = await _context.Bookings.AnyAsync(b =>
                b.VenueID == booking.VenueID &&
                b.BookingID != currentBookingId &&
                booking.StartDate < b.EndDate &&
                booking.EndDate > b.StartDate);

            if (clash)
            {
                ModelState.AddModelError(string.Empty, "This venue is already booked for the selected date and time. Please choose another venue or time.");
            }
        }
    }
}
