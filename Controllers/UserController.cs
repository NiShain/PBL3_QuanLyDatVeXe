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
            var user = context.Users.FirstOrDefault(u => u.ten == username && u.password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("userID", user.id.ToString());
                HttpContext.Session.SetString("userName", user.ten);
                HttpContext.Session.SetString("role", user.role);
                if (user.role == "admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
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
