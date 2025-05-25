using System.ComponentModel.DataAnnotations;

namespace PBL3_QuanLyDatXe.Models
{
    public class Account
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string ten { get; set; }

        [Required]
        public string password { get; set; }

        public string role { get; set; }

        public Customer Customer { get; set; }
    }
}
