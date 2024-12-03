using OMTS.DAL.Models;

namespace OMTS.UI.Models
{
	public class CinemaHallVM:BaseEntity
	{
		public string Name { get; set; }
		public string Location { get; set; }
		public int Capacity { get; set; }
	}
}
