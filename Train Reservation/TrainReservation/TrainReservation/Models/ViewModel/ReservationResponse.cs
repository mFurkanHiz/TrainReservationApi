namespace TrainReservation.Models.ViewModel
{
    public class ReservationResponse
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<ReservationDetail> YerlesimAyrinti { get; set; }
    }
}
