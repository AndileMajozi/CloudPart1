using EventEase.Data;
using EventEase.Models;
using EventEase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobStorageService;
        private const string PlaceholderImage = "https://images.unsplash.com/photo-1519167758481-83f550bb49b3?auto=format&fit=crop&w=900&q=80";

        public VenuesController(ApplicationDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.OrderBy(v => v.Name).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var venue = await _context.Venues.FirstOrDefaultAsync(v => v.VenueID == id);
            if (venue == null) return NotFound();
            return View(venue);
        }

        public IActionResult Create() => View(new Venue { ImageUrl = PlaceholderImage });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue, IFormFile? imageFile)
        {
            if (!ModelState.IsValid) return View(venue);

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    venue.ImageUrl = await _blobStorageService.UploadVenueImageAsync(imageFile);
                }
                else if (string.IsNullOrWhiteSpace(venue.ImageUrl))
                {
                    venue.ImageUrl = PlaceholderImage;
                }

                _context.Venues.Add(venue);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venue created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Image upload failed. Make sure Azurite is running. Details: {ex.Message}");
                return View(venue);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();
            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue, IFormFile? imageFile)
        {
            if (id != venue.VenueID) return NotFound();
            if (!ModelState.IsValid) return View(venue);

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    venue.ImageUrl = await _blobStorageService.UploadVenueImageAsync(imageFile);
                }
                else if (string.IsNullOrWhiteSpace(venue.ImageUrl))
                {
                    venue.ImageUrl = PlaceholderImage;
                }

                _context.Update(venue);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venue updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Venue could not be updated. Details: {ex.Message}");
                return View(venue);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var venue = await _context.Venues.FirstOrDefaultAsync(v => v.VenueID == id);
            if (venue == null) return NotFound();
            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasBookings = await _context.Bookings.AnyAsync(b => b.VenueID == id);
            if (hasBookings)
            {
                TempData["Error"] = "This venue cannot be deleted because it has active bookings.";
                return RedirectToAction(nameof(Index));
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Venue deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
