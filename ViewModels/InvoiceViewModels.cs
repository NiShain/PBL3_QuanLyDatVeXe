namespace PBL3_QuanLyDatXe.ViewModels
{
    public class InvoiceViewModels
    {
        public string tenKhachHang { get; set; }
        public string tenChuyenDi { get; set; }
        public int soGheDat { get; set; }
        public DateTime ngayDat { get; set; }
        public DateTime ngayDi { get; set; }
        public List<string> maCode { get; set; }
        public decimal tongTien { get; set; }
        public int TripID { get; set; }
        public bool IsPaid { get; set; }
    }
}
