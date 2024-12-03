using System.ComponentModel.DataAnnotations;

namespace OMTS.UI.Models
{
    public class AccountVM
    {
		[Required]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
		[Required]
		public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
		public string Password { get; set; }
		[Required]
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		public string ConfirmPassword { get; set; }
	}
}
