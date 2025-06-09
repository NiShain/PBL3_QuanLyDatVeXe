namespace PBL3_QuanLyDatXe.ViewModels
{
    public class AdminInvoiceViewModels
    {
        public string TenKhachHang { get; set; }
        public string TenChuyen { get; set; }
        public int SoLuongVe { get; set; }
        public bool IsPaid { get; set; }
    }

    public class AdminInvoiceListViewModels
    {
        public List<AdminInvoiceViewModels> Invoices { get; set; }
    }
}
