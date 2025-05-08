using System.ComponentModel.DataAnnotations;

namespace PBL3_QuanLyDatXe.ViewModels
{
    public class BusViewModels
    {
        [Required]
        public string tenXe { get; set; }
        [Required]
        public string bienSo { get; set; }
        [Required]
        public int soGhe { get; set; }
        [Required]
        public string loaiXe { get; set; }
    }
}
