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

        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.ToListAsync();
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Name = customerViewModel.Name,
                    Email = customerViewModel.Email,
                    Phone = customerViewModel.Phone
                };

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var customerViewModel = new CustomerViewModel
            {
                Id = customer.id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };

            return View(customerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();

                customer.Name = customerViewModel.Name;
                customer.Email = customerViewModel.Email;
                customer.Phone = customerViewModel.Phone;

                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerViewModel);
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