namespace PBL3_QuanLyDatXe.ViewModels
{
    public class RevenueViewModels
    {
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int SoLuongVeBan { get; set; }
        public decimal TongDoanhThu { get; set; }
        public List<DetailItem> ChiTiet { get; set; }

        public class DetailItem
        {
            public string TenChuyenDi { get; set; }
            public int SoVe { get; set; }
            public decimal DoanhThu { get; set; }
        }
    }
}
