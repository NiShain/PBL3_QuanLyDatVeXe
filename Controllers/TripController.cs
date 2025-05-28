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
        public bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }
        public async Task<IActionResult> Index()
        {
            
            var trips = await _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Bus)
                .ToListAsync();
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
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");
            ViewData["Busid"] = new SelectList(_context.Buses, "id", "tenXe");
            ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TripViewModels trip)
        {
            var bus = await _context.Buses.FindAsync(trip.Busid);
            var line = await _context.Lines.FindAsync(trip.Routeid);

            if (bus == null)
                ModelState.AddModelError("", "Không tìm thấy xe.");
            if (line == null)
                ModelState.AddModelError("", "Không tìm thấy tuyến.");
            if (trip.ngayDi.Date < DateTime.Today)
                ModelState.AddModelError("", "Ngày đi không được nhỏ hơn hôm nay.");

            if (!ModelState.IsValid)
            {
                ViewData["Busid"] = new SelectList(_context.Buses, "id", "tenXe", trip.Busid);
                ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen", trip.Routeid);
                return View(trip);
            }

            var trips = new Trip
            {
                Busid = trip.Busid,
                Routeid = trip.Routeid,
                ngayDi = trip.ngayDi,
                gioDi = trip.gioDi,
                giaVe = trip.giaVe,
                soGhe = bus.soGhe,
                sogheconTrong = bus.soGhe
            };

            _context.Add(trips);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();
            ViewData["Busid"] = new SelectList(_context.Buses, "id", "tenXe", trip.Busid);
            ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen", trip.Routeid);
            return View(trip);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Trip trip)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");
            if (!IsAdmin())
                return RedirectToAction("Login", "AccessDenied");
            if (id != trip.id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Busid"] = new SelectList(_context.Buses, "id", "tenXe", trip.Busid);
            ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen", trip.Routeid);
            return View(trip);
        }
        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.id == id);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var trip = await _context.Trips
                            .Include(t => t.Route)
                            .FirstOrDefaultAsync(m => m.id == id);
            if (trip == null) return NotFound();
            return View(trip);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");
            if (!IsAdmin())
                return RedirectToAction("Login", "AccessDenied");
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}