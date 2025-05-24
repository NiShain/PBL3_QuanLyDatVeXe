using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBL3_QuanLyDatXe.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CCCD { get; set; }

       [ForeignKey("Account")]
        public int UserId { get; set; }
        public Account Account { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
