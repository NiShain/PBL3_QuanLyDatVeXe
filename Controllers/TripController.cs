using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.ViewModels;
using PBL3_QuanLyDatXe.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class TripController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TripController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var trips = await _context.Trips.Include(t => t.Route).ToListAsync();
            return View(trips);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trip = await _context.Trips
                            .Include(t => t.Route)
                            .FirstOrDefaultAsync(m => m.id == id);
            if (trip == null) return NotFound();

            return View(trip);
        }

        public IActionResult Create()
        {
            ViewData["Busid"] = new SelectList(_context.Buses, "id", "tenXe");
            ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Trip trip)
        {
            if (ModelState.IsValid)
            {
                var bus = await _context.Buses.FindAsync(trip.Busid);
                if (bus == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy xe.");
                    ViewData["Busid"] = new SelectList(_context.Buses, "id", "tenXe", trip.Busid);
                    ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen", trip.Routeid);
                    return View(trip);
                }

                // Gán số ghế từ bus cho trip
                trip.soGhe = bus.soGhe;
                trip.sogheconTrong = bus.soGhe;
                _context.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Busid"] = new SelectList(_context.Buses, "id", "tenXe", trip.Busid);
            ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen", trip.Routeid);
            return View(trip);
        }
    }
}
