﻿using Microsoft.AspNetCore.Mvc;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TicketController(ApplicationDbContext context)
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
        public async Task<IActionResult> Create(int tripId, int seatNumber)
        {
            // Lấy UserId từ session
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized();

            // Tìm customer theo UserId (Account.id)
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer == null) return NotFound("Không tìm thấy khách hàng.");

            var trip = await _context.Trips
                .Include(t => t.Tickets)
                .FirstOrDefaultAsync(t => t.id == tripId);
            if (trip == null || trip.sogheconTrong <= 0)
                return BadRequest("Chuyến đi không hợp lệ hoặc đã hết chỗ.");

            if (trip.Tickets.Any(t => t.soGhe == seatNumber))
                return BadRequest("Ghế này đã được đặt.");

            string code = $"VE-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

            var ticket = new Ticket
            {
                Code = code,
                Customerid = customer.id,
                Tripid = trip.id,
                soGhe = seatNumber,
                ngayDat = DateTime.Now,
                trangThai = "Chưa thanh toán"
            };

            _context.Tickets.Add(ticket);
            trip.sogheconTrong -= 1;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, code = code });
        }

        public async Task<IActionResult> Details()
        {
            // Lấy UserId từ session
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized();

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
            if (customer != null)
            {
                var tickets = await _context.Tickets
                    .Include(t => t.Trip)
                    .Include(t => t.Trip.Route)
                    .Where(t => t.Customerid == customer.id).ToListAsync();

                return View(tickets);
            }

            return NotFound("Không tìm thấy khách hàng.");
        }
    }
}
