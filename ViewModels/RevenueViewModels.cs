namespace PBL3_QuanLyDatXe.ViewModels
{
    public class RevenueViewModels
    {
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int SoLuongVeBan { get; set; }
        public decimal TongDoanhThu { get; set; }
        public List<DetailItem> ChiTiet { get; set; }

        // Thêm các thuộc tính cho biểu đồ đường
        public List<RevenueByDateItem> DoanhThuTheoNgay { get; set; }
        public List<RevenueByMonthItem> DoanhThuTheoThang { get; set; }
        public List<RevenueByYearItem> DoanhThuTheoNam { get; set; }

        public class DetailItem
        {
            public string TenChuyenDi { get; set; }
            public int SoVe { get; set; }
            public decimal DoanhThu { get; set; }
        }

        public class RevenueByDateItem
        {
            public DateTime Ngay { get; set; }
            public decimal DoanhThu { get; set; }
        }

        public class RevenueByMonthItem
        {
            public int Nam { get; set; }
            public int Thang { get; set; }
            public decimal DoanhThu { get; set; }
        }

        public class RevenueByYearItem
        {
            public int Nam { get; set; }
            public decimal DoanhThu { get; set; }
        }
    }
}
