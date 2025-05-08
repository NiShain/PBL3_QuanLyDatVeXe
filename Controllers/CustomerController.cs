using Microsoft.AspNetCore.Mvc;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.ViewModels;
using PBL3_QuanLyDatXe.Data;
namespace PBL3_QuanLyDatXe.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("role");
            return role == "Admin";
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
        public async Task<IActionResult> Create(CustomerViewModels customer)
        {
            if (ModelState.IsValid)
            {
                var account = new Account
                {
                    ten = customer.CCCD,
                    password = customer.sodienthoai,
                    role = "Customer"
                };

                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();

                var Customer = new Customer
                {
                    hoten = customer.hoten,
                    sodienthoai = customer.sodienthoai,
                    email = customer.email,
                    CCCD = customer.CCCD,
                    UserId = account.id
                };
                await _context.Customers.AddAsync(Customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (IsAdmin())
            {
                TempData["Error"] = "ADMIN không được phép can thiệp vào thông tin của khách hàng";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("#", "#");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("#", "#");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
