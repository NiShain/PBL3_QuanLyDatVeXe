using PBL3_QuanLyDatXe.Models;

namespace PBL3_QuanLyDatXe.ViewModels
{
    public class TripViewModels
    {
        public int Busid { get; set; }

        public int Routeid { get; set; }
        
        public DateTime ngayDi { get; set; }
        public DateTime gioDi { get; set; }


        public int soGhe { get; set; }
        public int sogheconTrong { get; set; }

        public Decimal giaVe { get; set; }
    }
}
