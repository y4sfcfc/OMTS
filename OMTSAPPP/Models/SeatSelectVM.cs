namespace OMTS.UI.Models
{
	public class SeatSelectVM
	{
        public int SeatNo { get; set; }
        public int MovieId { get; set; }
		public int ShowtimeId {  get; set; }
        public int CustomerId { get; set; }
        public List<SeatVM> Seats { get; set; }
	}
}
