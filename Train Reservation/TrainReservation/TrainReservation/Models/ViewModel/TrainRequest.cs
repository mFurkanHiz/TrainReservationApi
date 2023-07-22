namespace TrainReservation.Models.ViewModel
{
    public class TrainRequest
    {
        public string Ad { get; set; }
        public List<VagonRequest> Vagonlar { get; set; }
    }
}
