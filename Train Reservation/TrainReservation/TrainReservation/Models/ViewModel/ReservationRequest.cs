namespace TrainReservation.Models.ViewModel
{
    public class ReservationRequest
    {
        public TrainRequest Tren { get; set; }
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }
}
