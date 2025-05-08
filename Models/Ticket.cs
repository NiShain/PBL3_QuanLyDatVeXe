using System.ComponentModel.DataAnnotations;

namespace PBL3_QuanLyDatXe.Models
{
    public class Ticket
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Code { get; set; }


        public int Tripid { get; set; }
        public Trip Trip { get; set; }

        public int Customerid { get; set; }
        public Customer Customer { get; set; }


        public int soGhe { get; set; }
        public DateTime ngayDat { get; set; }
        public string trangThai { get; set; } // Chưa thanh toán, đã thanh toán, đã hủy
    }
}
