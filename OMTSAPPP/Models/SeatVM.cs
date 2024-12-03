using OMTS.DAL.Models;

namespace OMTS.UI.Models
{
	public class SeatVM : BaseEntity
	{
		public int SeatNo { get; set; }
		public bool IsBooked { get; set; }
		public int CinemaHallId { get; set; }
		public string CinemaHallName { get; set; }
		public int ShowtimeId { get; set; }
		public int SeatId { get; set; }
		public List<CinemaHall> CinemaHalls { get; set; }

	}
}
