using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;

namespace OMTS.UI.ViewComponents
{
	public class BasketViewComponent : ViewComponent
	{
		private readonly IGenericRepository<Ticket> _ticketRepository;
		private readonly IGenericRepository<Payment> _paymentRepository;

		public BasketViewComponent(IGenericRepository<Ticket> ticketRepository, IGenericRepository<Payment> paymentRepository)
		{
			_ticketRepository = ticketRepository;
			_paymentRepository = paymentRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var tickets = await GetItemsAsync();

			List<TicketVM> list = new();
			foreach (var ticket in tickets)
			{

			/*	var isPaid = _paymentRepository.GetAll().Result.Any(p => p.TicketId == ticket.Id);*/
				list.Add(new TicketVM
				{
					IsPaid =ticket.IsPaid
				});
			}
			return View(list);
		}
		private async Task<List<Ticket>> GetItemsAsync()
		{
			var customerId = int.Parse(Request.Cookies["customer_id"]);

			return _ticketRepository.GetAll().Result.Where(x => x.CustomerId == customerId&&x.IsPaid==false).ToList();
		}
	}
}
