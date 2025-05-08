using System.ComponentModel.DataAnnotations;

namespace PBL3_QuanLyDatXe.Models
{
    public class Customer
    {
        [Key]
        public int id { get; set; }
        public string hoten { get; set; }
        public string sodienthoai { get; set; }
        public string email { get; set; }
        public string CCCD { get; set; }

        public int UserId { get; set; }
        public Account Account { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
