using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "AccessDenied");
            var customers = await _context.Customers.ToListAsync();
            return View(customers);
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
                var existedCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CCCD == customer.CCCD || c.Email == customer.Email || c.Phone == customer.Phone);
                if (existedCustomer != null)
                {
                    ModelState.AddModelError("", "Customer with the same CCCD, Email or Phone already exists.");
                    return View(customer);
                }

                
                var account = new Account
                {
                    ten = customer.Email,           
                    password = customer.Phone,      
                    role = "customer"
                };
                _context.Add(account);
                await _context.SaveChangesAsync();

                
                var customers = new Customer
                {
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    CCCD = customer.CCCD,
                    UserId = account.id            
                };

                _context.Customers.Add(customers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();
            var customerToUpdate = await _context.Customers.FindAsync(id);
            if (customerToUpdate == null) return NotFound();
            if (await TryUpdateModelAsync<Customer>(
                customerToUpdate,
                "",
                c => c.Name, c => c.Phone, c => c.Email, c => c.CCCD))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(customerToUpdate);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}