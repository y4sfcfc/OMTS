using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Areas.Admin.Models;
using OMTS.UI.Models;

namespace OMTS.UI.Controllers
{
	public class AccountController : Controller
	{
		private readonly IGenericRepository<Customer> _customerRepository;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public AccountController(IGenericRepository<Customer> customerRepository,
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager)
		{
			_customerRepository = customerRepository;
			_userManager = userManager;
			_signInManager = signInManager;
		}
		public IActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(AccountVM model)
		{
			if (ModelState.IsValid)
			{
				IdentityUser identityUser = new()
				{
					Email = model.Email,
					UserName = model.UserName,
				};
				var result = await _userManager.CreateAsync(identityUser, model.Password);
				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(identityUser, isPersistent: false);
					Customer customer = new()
					{
						Email = model.Email,
						PhoneNumber = model.PhoneNumber,
						UserName = model.UserName,
						User = identityUser,
						UserId = identityUser.Id
					};
					CookieOptions options = new CookieOptions
					{
						Expires = DateTime.Now.AddDays(5)
					};
					var logedInCustomer = await _customerRepository.Add(customer);
					await _customerRepository.SaveAsync();
					Response.Cookies.Append("customer_id", logedInCustomer.Id.ToString(), options);
					Response.Cookies.Append("email", identityUser.Email, options);
					Response.Cookies.Append("userName", identityUser.UserName, options);
					return RedirectToAction("Index", "Movies", new { customerId = logedInCustomer.Id });
				}
				else
				{

					foreach (var item in result.Errors)
					{
						ModelState.AddModelError("", item.Description);
					}
				}
			}
			else
			{
				return View(model);
			}
			return View();
		}

		public IActionResult LogIn()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> LogIn(AccountVM model)
		{

			IdentityUser identityUser = new()
			{
				Email = model.Email,
				UserName = model.UserName,
				PhoneNumber = model.PhoneNumber
			};
			var user = await _userManager.FindByEmailAsync(model.Email);

			await _signInManager.SignInAsync(user, false);

			CookieOptions options = new CookieOptions
			{
				Expires = DateTime.Now.AddDays(5)
			};
			var logedInCustomer = _customerRepository.GetAll().Result.First(c => c.UserId == user.Id);
			Response.Cookies.Append("customer_id", logedInCustomer.Id.ToString(), options);
			Response.Cookies.Append("email", user.Email, options);
			Response.Cookies.Append("userName", user.UserName, options);
			return RedirectToAction("Index", "Movies", new { customerId = logedInCustomer.Id });


			/*var cutomers = await _customerRepository.GetAll();
            foreach (var customer in cutomers)
            {
                if (model.Email == customer.Email)
                {
                    string key = "customer_id";
                    string value = customer.Id.ToString();
                    CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(5)
                    };
                    Response.Cookies.Append(key, value, options);
                    return RedirectToAction("Index", "Movies", new { customerId = customer.Id });
                }
            }*/
			return View(model);
		}
	}
}
