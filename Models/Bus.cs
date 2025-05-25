using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace PBL3_QuanLyDatXe.Models
{
    public class Bus
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string tenXe { get; set; }
        [Required]
        public string bienSo { get; set; }
        [Required]
        public int soGhe { get; set; }
        [Required]
        public string loaiXe { get; set; }

        public ICollection<Trip>? Trips { get; set; }
    }
}
