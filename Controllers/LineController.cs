using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.ViewModels;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class LineController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LineController(ApplicationDbContext context)
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
            var lines = await _context.Lines.ToListAsync();
            return View(lines);
        }
        public IActionResult Create()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");
            return View();
        }
        [HttpPost]
        public IActionResult Create(LineViewModels line)
        {

            if (ModelState.IsValid)
            {
                var lines = new Line
                {
                    tenTuyen = line.tenTuyen,
                    diemDi = line.diemDi,
                    diemDen = line.diemDen,
                };
                _context.Lines.Add(lines);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(line);
        }
        public IActionResult Edit(int id)
        {
            var line = _context.Lines.FirstOrDefaultAsync(x => x.id == id);
            if (line == null)
            {
                return NotFound();
            }
            return View(line);
        }
        [HttpPost]
        public IActionResult Edit(Line line)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");
            if (!IsAdmin())
                return RedirectToAction("Login", "AccessDenied");
            if (ModelState.IsValid)
            {
                _context.Update(line);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(line);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var line = await _context.Lines.FirstOrDefaultAsync(x => x.id == id);
            if (line == null)
            {
                return NotFound();
            }
            return View(line);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");
            if (!IsAdmin())
                return RedirectToAction("Login", "AccessDenied");
            var line = await _context.Lines.FirstOrDefaultAsync(x => x.id == id);
            if (line != null)
            {
                _context.Lines.Remove(line);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
