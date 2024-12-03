using OMTS.DAL.Models;

namespace OMTS.UI.Models
{
	public class TicketVM : BaseEntity
	{
		public int MovieId { get; set; }
		public string MovieName { get; set; }
		public int? CustomerId { get; set; }
		public string? CustomerName { get; set; }
		public int ShowtimeId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int SeatNumber { get; set; }
		public decimal Price { get; set; }
		public List<Seat> SeatNumbers { get; set; }
		public List<Movie> Movies { get; set; }
		public List<Showtime> Showtimes { get; set; }

        public bool IsPaid { get; set; }
    }
}
