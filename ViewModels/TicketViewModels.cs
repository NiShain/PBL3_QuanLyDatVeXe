using PBL3_QuanLyDatXe.Models;

namespace PBL3_QuanLyDatXe.ViewModels
{
    public class TicketViewModels
    {
        public int SelectedRouteId { get; set; }
        public int SelectedTripId { get; set; }
        public int SelectedSeat { get; set; }
        public List<Trip> AvailableTrips { get; set; }
        public List<int> BookedSeats { get; set; }
    }
}
