using Microsoft.AspNetCore.Mvc;

namespace PBL3_QuanLyDatXe.Controllers
{
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
