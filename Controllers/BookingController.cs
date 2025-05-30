using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.ViewModels;

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

        // Chọn số lượng ghế muốn đặt
        public IActionResult SelectQuantity(int tripId)
        {
            var trip = _context.Trips.FirstOrDefault(t => t.id == tripId);
            if (trip == null)
            {
                return NotFound("Chuyến đi không tồn tại.");
            }
            ViewBag.TripId = tripId;
            ViewBag.SoGheConTrong = trip.sogheconTrong;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int tripId, int soLuongGhe)
        {
            // Lấy UserId từ session
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập nếu không có UserId
            }

            // Tìm customer theo UserId (Account.id)
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null)
            {
                return NotFound("Không tìm thấy thông tin khách hàng.");
            }

            var trip = await _context.Trips
                .Include(t => t.Tickets)
                .Include(t => t.Route)
                .FirstOrDefaultAsync(t => t.id == tripId);

            if (trip == null)
            {
                return NotFound("Chuyến đi không tồn tại.");
            }

            if (trip.sogheconTrong < soLuongGhe)
            {
                return BadRequest("Số lượng ghế trống không đủ.");
            }

            // Lấy danh sách ghế đã đặt
            var bookedSeats = trip.Tickets?.Select(t => t.soGhe).ToList() ?? new List<int>();
            var availableSeats = Enumerable.Range(1, trip.soGhe).Except(bookedSeats).Take(soLuongGhe).ToList();

            if (availableSeats.Count < soLuongGhe)
            {
                return BadRequest("Không đủ ghế trống.");
            }

            var maCodeList = new List<string>();
            var ngayDat = DateTime.Now;

            foreach (var seat in availableSeats)
            {
                string code = $"VE-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
                var ticket = new Ticket
                {
                    Tripid = tripId,
                    Customerid = customer.id,
                    soGhe = seat,
                    ngayDat = ngayDat,
                    trangThai = "Chưa thanh toán",
                    Code = code
                };
                _context.Tickets.Add(ticket);
                maCodeList.Add(code);
            }

            trip.sogheconTrong -= soLuongGhe;
            await _context.SaveChangesAsync();

            // Tạo ViewModel hóa đơn
            var invoice = new InvoiceViewModels
            { 
                tenKhachHang = customer.Name,
                tenChuyenDi = trip.Route?.tenTuyen ?? "",
                soGheDat = soLuongGhe,
                ngayDat = ngayDat,
                ngayDi = trip.ngayDi,
                maCode = maCodeList,
                tongTien = trip.giaVe * soLuongGhe
            };

            return View("Invoice", invoice);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
