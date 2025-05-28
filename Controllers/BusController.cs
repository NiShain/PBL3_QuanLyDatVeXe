using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.ViewModels;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.Data;
using System.Threading.Tasks;


namespace PBL3_QuanLyDatXe.Controllers
{
    public class BusController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BusController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Buses = await _context.Buses.ToListAsync();
            return View(Buses);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BusViewModels bus)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                var Bus = new Bus
                {
                    tenXe = bus.tenXe,
                    bienSo = bus.bienSo,
                    soGhe = bus.soGhe,
                    loaiXe = bus.loaiXe
                };
                await _context.Buses.AddAsync(Bus);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bus);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var bus = await _context.Buses.FirstOrDefaultAsync(x => x.id == id);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Bus bus)
        {
            
            if (ModelState.IsValid)
            {
                _context.Buses.Update(bus);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bus);
        }
        public async Task<IActionResult> Delete(int id)
        {
            
            var bus = await _context.Buses.FirstOrDefaultAsync(x => x.id == id);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var bus = await _context.Buses.FirstOrDefaultAsync(x => x.id == id);
            if (bus != null)
            {
                _context.Buses.Remove(bus);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
