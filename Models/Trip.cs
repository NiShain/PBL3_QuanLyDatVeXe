using System.ComponentModel.DataAnnotations;

namespace PBL3_QuanLyDatXe.Models
{
    public class Trip
    {
        [Key]
        public int id { get; set; }

        public int Routeid { get; set; }
        public Line Route { get; set; }


        public DateTime ngayDi { get; set; }
        public DateTime gioDi { get; set; }


        public int soGhe { get; set; }
        public int sogheconTrong { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }

    }
}
