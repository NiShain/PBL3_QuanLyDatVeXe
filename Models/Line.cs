namespace PBL3_QuanLyDatXe.Models
{
    public class Line
    {
        public int Id { get; set; }
        public string tenTuyen { get; set; }
        public string diemDi { get; set; }
        public string diemDen { get; set; }

        //Relation
        public ICollection<Trip>? Trips { get; set; }

    }
}
