using OMTS.DAL.Models;

namespace OMTS.UI.Models
{
	public class ReviewVM:BaseEntity
	{
		public int MovieId { get; set; }
		public Movie Movie { get; set; }
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }
		public int Rating { get; set; } 
		public string Comment { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
