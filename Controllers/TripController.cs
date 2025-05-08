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
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //public IActionResult Create()
        //{
        //    ViewData["Routeid"] = new SelectList(_context.Lines, "id", "id");
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(Trip trip)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Trips.Add(trip);
        //        _context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewData["Routeid"] = new SelectList(_context.Lines, "id", "id", trip.Routeid);
        //    return View(trip);
        //}
        //public IActionResult Edit(int id)
        //{
        //    var trip = _context.Trips.Find(id);
        //    if (trip == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(trip);
        //}
        //[HttpPost]
        //public IActionResult Edit(Trip trip)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Entry(trip).State = EntityState.Modified;
        //        _context.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(trip);
        //}
        //public IActionResult Delete(int id)
        //{
        //    var trip = _context.Trips.Find(id);
        //    if (trip == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(trip);
        //}
        //[HttpPost]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var trip = _context.Trips.Find(id);
        //    if (trip == null)
        //    {
        //        return NotFound();
        //    }
        //    _context.Trips.Remove(trip);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");

        //}
        public async Task<IActionResult> Index()
        {
            var trips = await _context.Trips.Include(t => t.Route).ToListAsync();
            return View(trips);
        }

        // GET: Trip/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trip = await _context.Trips
                            .Include(t => t.Route)
                            .FirstOrDefaultAsync(m => m.id == id);
            if (trip == null) return NotFound();

            return View(trip);
        }

        // GET: Trip/Create
        public IActionResult Create()
        {
            ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen");
            return View();
        }

        // POST: Trip/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Routeid,ngayDi,gioDi,soGhe,sogheconTrong")] Trip trip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Routeid"] = new SelectList(_context.Lines, "id", "tenTuyen", trip.Routeid);
            return View(trip);
        }
    }
}
