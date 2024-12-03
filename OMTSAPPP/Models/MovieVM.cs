using OMTS.DAL.Models;

namespace OMTS.UI.Models
{
	public class MovieVM:BaseEntity
	{
        public string Title { get; set; }
		public string Genre { get; set; }
		public int Duration { get; set; }
		public string Director { get; set; }
		public DateTime ReleaseDate { get; set; }
        public int? CustomerId { get; set; }
        public string Thumbnail { get; set; }
		public IFormFile Image { get; set; }
		public float Average {  get; set; }
    }
}
