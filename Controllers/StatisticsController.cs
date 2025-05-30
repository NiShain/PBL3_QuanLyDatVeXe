using Microsoft.AspNetCore.Mvc;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }


        public IActionResult Revenue()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Account");
            var model = new RevenueViewModels
            {
                NgayBatDau = DateTime.Now.AddDays(-30), // Mặc định 30 ngày trước
                NgayKetThuc = DateTime.Now,
                SoLuongVeBan = 0,
                TongDoanhThu = 0,
                ChiTiet = new List<RevenueViewModels.DetailItem>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Revenue(DateTime? from, DateTime? to)
        {
            var query = _context.Tickets
                .Include(t => t.Trip)
                .Where(t => t.trangThai == "Đã thanh toán");

            if (from.HasValue)
                query = query.Where(t => t.ngayDat >= from.Value);
            if (to.HasValue)
                query = query.Where(t => t.ngayDat <= to.Value);

            var tickets = await query.ToListAsync();

            var model = new RevenueViewModels
            {
                NgayBatDau = from,
                NgayKetThuc = to,
                SoLuongVeBan = tickets.Count,
                TongDoanhThu = tickets.Sum(t => t.Trip.giaVe),
                ChiTiet = tickets
                    .GroupBy(t => t.Trip.Route.tenTuyen)
                    .Select(g => new RevenueViewModels.DetailItem
                    {
                        TenChuyenDi = g.Key,
                        SoVe = g.Count(),
                        DoanhThu = g.Sum(t => t.Trip.giaVe)
                    }).ToList()
            };

            return View(model);
        }
    }
}
