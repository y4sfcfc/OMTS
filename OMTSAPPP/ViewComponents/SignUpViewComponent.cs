using Microsoft.AspNetCore.Mvc;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;

namespace OMTS.UI.ViewComponents
{
	public class SignUpViewComponent:ViewComponent
	{
		private readonly IGenericRepository<Customer> _customerRepository;

		public SignUpViewComponent(IGenericRepository<Customer> customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var customer = await GetItemsAsync();
			return View();
			

		}
		private async Task<Customer> GetItemsAsync()
		{
			var customerId = int.Parse(Request.Cookies["customer_id"]);

			return   await _customerRepository.Get(customerId);
		}
	}
}
