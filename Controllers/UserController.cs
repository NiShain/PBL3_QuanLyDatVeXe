using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.Models;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext context;
        public UserController(ApplicationDbContext _context)
        {
            context = _context;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName == username && u.PasswordHash == password);
            if (user != null)
            {
                HttpContext.Session.SetString("userID", user.Id.ToString());
                HttpContext.Session.SetString("userName", user.UserName);

                // Fix: Add a custom property for 'Role' in your user model or use a derived class
                if (user is ApplicationUser appUser) // Assuming ApplicationUser is a derived class with a 'Role' property
                {
                    HttpContext.Session.SetString("role", appUser.Role);
                    if (appUser.Role == "admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewBag.Error = "User role is not defined.";
                return View();
            }
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
