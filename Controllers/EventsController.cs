using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string PlaceholderImage = "https://images.unsplash.com/photo-1505236858219-8359eb29e329?auto=format&fit=crop&w=900&q=80";

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.OrderBy(e => e.EventName).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.Events.FirstOrDefaultAsync(e => e.EventID == id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View(new Event { ImageUrl = PlaceholderImage });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event item)
        {
            if (!ModelState.IsValid) return View(item);
            if (string.IsNullOrWhiteSpace(item.ImageUrl)) item.ImageUrl = PlaceholderImage;
            _context.Events.Add(item);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.Events.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event item)
        {
            if (id != item.EventID) return NotFound();
            if (!ModelState.IsValid) return View(item);
            if (string.IsNullOrWhiteSpace(item.ImageUrl)) item.ImageUrl = PlaceholderImage;
            _context.Update(item);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.Events.FirstOrDefaultAsync(e => e.EventID == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasBookings = await _context.Bookings.AnyAsync(b => b.EventID == id);
            if (hasBookings)
            {
                TempData["Error"] = "This event cannot be deleted because it has active bookings.";
                return RedirectToAction(nameof(Index));
            }

            var item = await _context.Events.FindAsync(id);
            if (item != null)
            {
                _context.Events.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Event deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
