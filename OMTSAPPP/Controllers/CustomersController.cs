using Microsoft.AspNetCore.Mvc;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;

namespace OMTS.UI.Controllers
{
	public class CustomersController : Controller
	{
		private readonly IGenericRepository<Customer> _customerRepository;

		public CustomersController(IGenericRepository<Customer> customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<IActionResult> Details()
		{
			int customerId = int.Parse(Request.Cookies["customer_id"]);
			var customer = await _customerRepository.Get(customerId);
			CustomerVM customerVM = new()
			{
				Id = customerId,
				Name = customer.UserName,
				Email = customer.Email,
				PhoneNumber = customer.PhoneNumber,
			};
			return View(customerVM);
		}
		public async Task<IActionResult> Edit(int id)
		{
			var customer = await _customerRepository.Get(id);
			CustomerVM customerVM = new()
			{
				Id = customer.Id,
				Name = customer.UserName,
				Email = customer.Email,
				PhoneNumber = customer.PhoneNumber,
			};
			return View(customerVM);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(CustomerVM model)
		{

			var customer = await _customerRepository.Get(model.Id);
			customer.UserName= model.Name;
			customer.Email= model.Email;
			customer.PhoneNumber= model.PhoneNumber;
			_customerRepository.Update(customer);
			await _customerRepository.SaveAsync();
			return RedirectToAction("Index","Movies");
		}
	}
}
