using OMTS.DAL.Models;

namespace OMTS.UI.Models
{
	public class ShowtimeVM : BaseEntity
	{
		public int MovieId { get; set; }
		public string MovieTitle { get; set; }
		public List<Movie> Movies { get; set; }
		public int CinemaHallId { get; set; }
		public string CinemaHallName { get; set; }
		public List<CinemaHall> CinemaHalls { get; set; }
		public int CustomerId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}
