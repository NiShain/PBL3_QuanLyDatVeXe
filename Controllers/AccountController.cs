using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using PBL3_QuanLyDatXe.Models;
using PBL3_QuanLyDatXe.ViewModels;
using PBL3_QuanLyDatXe.Data;
using Microsoft.EntityFrameworkCore;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string ten, string password)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.ten == ten && a.password == password);

            if (account != null)
            {
                HttpContext.Session.SetString("UserId", account.id.ToString());
                HttpContext.Session.SetString("Username", account.ten);
                HttpContext.Session.SetString("Role", account.role);

                if (account.role == "Admin") // Fix logic for role comparison
                {
                    return RedirectToAction("Index", "Trip");
                }
                else if (account.role == "customer") // Fix logic for role comparison
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
