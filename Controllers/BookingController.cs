using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.Models.Vnpay;
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


        public async Task<IActionResult> AdminInvoiceList(string sortOrder, string searchTerm)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StatusSortParam"] = sortOrder == "status" ? "status_desc" : "status";
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentSearchTerm"] = searchTerm;

            
            var tickets = await _context.Tickets
                .Include(t => t.Customer)
                .Include(t => t.Trip)
                    .ThenInclude(trip => trip.Route)
                .OrderByDescending(t => t.ngayDat)
                .ToListAsync();

            
            var grouped = tickets.GroupBy(t => new { t.Customerid, t.Tripid, t.ngayDat });

            var invoices = new List<InvoiceViewModels>();
            foreach (var group in grouped)
            {
                var firstTicket = group.First();
                var trip = firstTicket.Trip;
                var customer = firstTicket.Customer;
                var maCodeList = group.Select(t => t.Code).ToList();
                bool isPaid = group.All(t => t.trangThai == "Đã thanh toán");

                invoices.Add(new InvoiceViewModels
                {
                    tenKhachHang = customer.Name,
                    tenChuyenDi = trip.Route?.tenTuyen ?? "",
                    soGheDat = group.Count(),
                    ngayDat = group.Key.ngayDat,
                    ngayDi = trip.ngayDi,
                    maCode = maCodeList,
                    tongTien = trip.giaVe * group.Count(),
                    TripID = trip.id,
                    IsPaid = isPaid
                });
            }

            // Áp dụng tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchTerm))
            {
                invoices = invoices.Where(i =>
                    i.tenKhachHang.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Áp dụng sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    invoices = invoices.OrderByDescending(i => i.tenKhachHang).ToList();
                    break;
                case "status":
                    invoices = invoices.OrderBy(i => i.IsPaid).ToList();
                    break;
                case "status_desc":
                    invoices = invoices.OrderByDescending(i => i.IsPaid).ToList();
                    break;
                default:
                    invoices = invoices.OrderBy(i => i.tenKhachHang).ToList();
                    break;
            }

            var model = new InvoiceListViewModels
            {
                Invoices = invoices
            };

            return View(model);
        }
        public async Task<IActionResult> SelectRoute(string diemDi, string diemDen, string sortOrder)
        {
            // Lưu trạng thái tìm kiếm và sắp xếp
            ViewData["CurrentDiemDi"] = diemDi;
            ViewData["CurrentDiemDen"] = diemDen;
            ViewData["TenTuyenSortParam"] = string.IsNullOrEmpty(sortOrder) ? "tenTuyen_desc" : "";
            ViewData["DiemDiSortParam"] = sortOrder == "diemDi" ? "diemDi_desc" : "diemDi";
            ViewData["DiemDenSortParam"] = sortOrder == "diemDen" ? "diemDen_desc" : "diemDen";

            // Query cơ bản
            var routes = _context.Lines.AsQueryable();

            // Áp dụng lọc theo điểm đi và điểm đến nếu có
            if (!string.IsNullOrEmpty(diemDi))
            {
                routes = routes.Where(r => r.diemDi.Contains(diemDi));
            }

            if (!string.IsNullOrEmpty(diemDen))
            {
                routes = routes.Where(r => r.diemDen.Contains(diemDen));
            }

            // Áp dụng sắp xếp
            switch (sortOrder)
            {
                case "tenTuyen_desc":
                    routes = routes.OrderByDescending(r => r.tenTuyen);
                    break;
                case "diemDi":
                    routes = routes.OrderBy(r => r.diemDi);
                    break;
                case "diemDi_desc":
                    routes = routes.OrderByDescending(r => r.diemDi);
                    break;
                case "diemDen":
                    routes = routes.OrderBy(r => r.diemDen);
                    break;
                case "diemDen_desc":
                    routes = routes.OrderByDescending(r => r.diemDen);
                    break;
                default:
                    // Mặc định sắp xếp theo tên tuyến tăng dần
                    routes = routes.OrderBy(r => r.tenTuyen);
                    break;
            }

            return View(await routes.ToListAsync());
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
        [HttpGet]
        public async Task<IActionResult> ConfirmBooking()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null)
            {
                return NotFound("Không tìm thấy thông tin khách hàng.");
            }

            // Lấy tất cả vé của khách hàng, bao gồm thông tin chuyến và tuyến
            var tickets = await _context.Tickets
                .Where(t => t.Customerid == customer.id)
                .Include(t => t.Trip)
                    .ThenInclude(trip => trip.Route)
                .OrderByDescending(t => t.ngayDat)
                .ToListAsync();

            if (!tickets.Any())
            {
                return Content("Bạn chưa có hóa đơn nào.");
            }

            // Nhóm các vé theo từng lần đặt (ngayDat)
            var grouped = tickets.GroupBy(t => new { t.Tripid, t.ngayDat })
                .OrderByDescending(g => g.Key.ngayDat);

            var invoices = new List<InvoiceViewModels>();
            foreach (var group in grouped)
            {
                var firstTicket = group.First();
                var trip = firstTicket.Trip;
                var maCodeList = group.Select(t => t.Code).ToList();

                // Kiểm tra nếu tất cả vé trong group đã thanh toán
                bool isPaid = group.All(t => t.trangThai == "Đã thanh toán");

                invoices.Add(new InvoiceViewModels
                {
                    tenKhachHang = customer.Name,
                    tenChuyenDi = trip.Route?.tenTuyen ?? "",
                    soGheDat = group.Count(),
                    ngayDat = group.Key.ngayDat,
                    ngayDi = trip.ngayDi,
                    maCode = maCodeList,
                    tongTien = trip.giaVe * group.Count(),
                    TripID = trip.id, // Thêm dòng này
                    IsPaid = isPaid
                });
            }

            var model = new InvoiceListViewModels
            {
                Invoices = invoices
            };

            return View("InvoiceList", model); // Tạo view mới InvoiceList.cshtml
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

            return View(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> PayInvoice(int tripId, string firstTicketCode)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return RedirectToAction("Login", "Account");

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null)
                return NotFound("Không tìm thấy thông tin khách hàng.");

            // Tìm vé cụ thể bằng mã vé đầu tiên
            var firstTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Customerid == customer.id && t.Tripid == tripId && t.Code == firstTicketCode);

            if (firstTicket == null)
                return NotFound("Không tìm thấy hóa đơn.");

            // Tìm tất cả vé cùng thời gian đặt chính xác
            var tickets = await _context.Tickets
                .Where(t => t.Customerid == customer.id && t.Tripid == tripId && t.ngayDat == firstTicket.ngayDat)
                .Include(t => t.Trip)
                .ToListAsync();

            var totalAmount = tickets.First().Trip.giaVe * tickets.Count;

            // Tạo mô tả đơn hàng với đầy đủ thông tin và thêm mã vé đầu tiên
            var model = new PaymentInformationModel
            {
                OrderType = "billpayment",
                Amount = (double)totalAmount,
                OrderDescription = $"{tripId}|{firstTicket.ngayDat:yyyy-MM-dd HH:mm:ss}|{firstTicketCode}",
                Name = customer.Name
            };

            ViewBag.PaymentModel = model;
            return View("~/Views/Shared/PaymentRedirect.cshtml");
        }

    }

}
