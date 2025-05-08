using System.ComponentModel.DataAnnotations;

namespace PBL3_QuanLyDatXe.ViewModels
{
    public class CustomerViewModels
    {
        [Required]
        public string hoten { get; set; }
        [Required]
        public string sodienthoai { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string CCCD { get; set; }
    }
}
