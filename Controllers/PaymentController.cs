using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.ViewModels;
using PBL3_QuanLyDatXe.Models.Vnpay;
using PBL3_QuanLyDatXe.Services.Vnpay;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService, ApplicationDbContext context)
        {

            _vnPayService = vnPayService;
            _context = context;
        }

        [HttpPost]
        [Route("CreatePaymentUrl")]
        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        //[HttpGet]
        //public async Task<IActionResult> PaymentCallbackVnpay()
        //{
        //    var response = _vnPayService.PaymentExecute(Request.Query);

        //    // Lấy lại thông tin group từ TempData (hoặc truyền qua vnp_OrderInfo nếu cần)
        //    if (TempData["PayTripId"] != null && TempData["PayNgayDat"] != null && response.Success)
        //    {
        //        int tripId = int.Parse(TempData["PayTripId"].ToString());
        //        DateTime ngayDat = DateTime.Parse(TempData["PayNgayDat"].ToString());

        //        var userIdString = HttpContext.Session.GetString("UserId");
        //        if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
        //        {
        //            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        //            if (customer != null)
        //            {
        //                var tickets = await _context.Tickets
        //                    .Where(t => t.Customerid == customer.id && t.Tripid == tripId && t.ngayDat == ngayDat)
        //                    .ToListAsync();

        //                foreach (var ticket in tickets)
        //                {
        //                    ticket.trangThai = "Đã thanh toán";
        //                }
        //                await _context.SaveChangesAsync();
        //            }
        //        }
        //    }

        //    return Json(response);
        //}
        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            try
            {
                // Log tất cả các query parameters để debug
                foreach (var key in Request.Query.Keys)
                {
                    Console.WriteLine($"Query param: {key} = {Request.Query[key]}");
                }

                var response = _vnPayService.PaymentExecute(Request.Query);
                Console.WriteLine($"Response: Success={response.Success}, Code={response.VnPayResponseCode}, OrderInfo={response.OrderDescription}");

                if (response.Success && response.VnPayResponseCode == "00")
                {
                    try
                    {
                        var orderInfoParts = response.OrderDescription.Split('|');
                        if (orderInfoParts.Length >= 3)  // Phải có đủ 3 phần: tripId, ngayDat, firstTicketCode
                        {
                            int tripId = int.Parse(orderInfoParts[0]);
                            DateTime ngayDat = DateTime.Parse(orderInfoParts[1]);
                            string firstTicketCode = orderInfoParts[2];  // Thêm mã vé đầu tiên để xác định chính xác nhóm vé

                            var userIdString = HttpContext.Session.GetString("UserId");
                            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
                            {
                                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
                                if (customer != null)
                                {
                                    // QUAN TRỌNG: Chỉ cập nhật vé có cùng mã Code hoặc cùng thời gian đặt chính xác
                                    var firstTicket = await _context.Tickets
                                        .FirstOrDefaultAsync(t => t.Code == firstTicketCode && t.Tripid == tripId);

                                    if (firstTicket != null)
                                    {
                                        var exactDateTime = firstTicket.ngayDat;

                                        // Tìm tất cả vé được đặt cùng lúc với vé đầu tiên
                                        var tickets = await _context.Tickets
                                            .Where(t => t.Customerid == customer.id &&
                                                      t.Tripid == tripId &&
                                                      t.ngayDat == exactDateTime)
                                            .ToListAsync();

                                        if (tickets.Any())
                                        {
                                            foreach (var ticket in tickets)
                                            {
                                                ticket.trangThai = "Đã thanh toán";
                                            }
                                            await _context.SaveChangesAsync();
                                            Console.WriteLine($"Updated {tickets.Count} tickets to 'Đã thanh toán'");
                                            return RedirectToAction("PaymentSuccess", new { tripId = tripId });
                                        }
                                        else
                                        {
                                            Console.WriteLine($"No tickets found for customer {customer.id}, trip {tripId}, date {exactDateTime}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"First ticket with code {firstTicketCode} not found");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid order description format: " + response.OrderDescription);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing payment: {ex.Message}");
                    }
                }

                ViewBag.Message = response.Success ? "Thanh toán thành công" : "Thanh toán thất bại";
                return View("PaymentResult", response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in callback: {ex.Message}");
                ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý thanh toán";
                return View("PaymentResult", new PaymentResponseModel { Success = false });
            }
        }
        // Thêm action xác nhận thanh toán thành công
        public IActionResult PaymentSuccess(int tripId)
        {
            ViewBag.TripId = tripId;
            return View();
        }

    }
}
