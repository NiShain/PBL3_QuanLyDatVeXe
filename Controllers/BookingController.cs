using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.Models;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class BookingController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectRoute()
        {
            var routes = _context.Lines.ToList();
            return View(routes);
        }

        public IActionResult SelectTrip(int routeId)
        {
            var trips = _context.Trips.Where(t => t.Routeid == routeId && t.sogheconTrong > 0).ToList();
            return View(trips);
        }

        public IActionResult SelectSeat(int tripId)
        {
            var trip = _context.Trips.Include(t => t.Tickets).FirstOrDefault(t => t.id == tripId);
            var bookedSeats = trip.Tickets.Select(t => t.soGhe).ToList();
            ViewBag.TripId = tripId;
            ViewBag.BookedSeats = bookedSeats;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int tripId, int selectedSeat)
        {
            // Lấy ID khách hàng từ session hoặc User.Identity
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ten == User.Identity.Name);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.id);

            // Tạo mã vé
            string code = $"VE-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

            var ticket = new Ticket
            {
                Tripid = tripId,
                Customerid = customer.id,
                soGhe = selectedSeat,
                ngayDat = DateTime.Now,
                trangThai = "Chưa thanh toán",
                Code = code
            };

            _context.Tickets.Add(ticket);

            // Cập nhật ghế còn trống
            var trip = await _context.Trips.FindAsync(tripId);
            trip.sogheconTrong -= 1;

            await _context.SaveChangesAsync();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
