using Microsoft.AspNetCore.Identity;

namespace PBL3_QuanLyDatXe.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }

        public string Role { get; set; } // Ví dụ: "Admin", "User", v.v.
    }
}
