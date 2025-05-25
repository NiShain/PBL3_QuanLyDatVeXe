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
            if (trip == null)
            {
                return NotFound("Chuyến đi không tồn tại.");
            }
            var bookedSeats = trip.Tickets?.Select(t => t.soGhe).ToList() ?? new List<int>();
            ViewBag.TripId = tripId;
            ViewBag.BookedSeats = bookedSeats;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int tripId, int selectedSeat)
        {
            if (User.Identity == null || string.IsNullOrEmpty(User.Identity.Name))
            {
                return Unauthorized("Người dùng chưa đăng nhập.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng.");
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == int.Parse(user.Id));

            if (customer == null) {
                return NotFound("Không tìm thấy thông tin khách hàng.");
            }

            var trip = await _context.Trips
                .Include(t => t.Tickets)
                .FirstOrDefaultAsync(t => t.id == tripId);

            if (trip == null)
            {
                return NotFound("Chuyến đi không tồn tại.");
            }

            if (trip.sogheconTrong <= 0)
            {
                return BadRequest("Không còn ghế trống.");
            }

            bool isSeatTaken = (trip.Tickets?.Any(t => t.soGhe == selectedSeat)) ?? false;
            if (isSeatTaken)
            {
                return BadRequest("Ghế đã được đặt, vui lòng chọn ghế khác.");
            }

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
