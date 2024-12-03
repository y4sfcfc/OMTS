using OMTS.DAL.Models;

namespace OMTS.UI.Models
{
	public class CustomerVM:BaseEntity
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
	}
}
