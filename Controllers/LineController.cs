using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.Models;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class LineController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LineController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Line line)
        {
            if (ModelState.IsValid)
            {
                _context.Lines.Add(line);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(line);
        }
        public IActionResult Edit(int id)
        {
            var line = _context.Lines.Find(id);
            if (line == null)
            {
                return NotFound();
            }
            return View(line);
        }
        [HttpPost]
        public IActionResult Edit(Line line)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(line).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(line);
        }
    }
}
