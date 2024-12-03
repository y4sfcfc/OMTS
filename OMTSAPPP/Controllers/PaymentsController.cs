using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;
using System.Collections.Generic;
using System.Linq;

namespace OMTS.UI.Controllers
{
	public class PaymentsController : Controller
	{
		private readonly IGenericRepository<Ticket> _ticketRepository;
		private readonly IGenericRepository<Payment> _paymentRepository;
		private readonly IGenericRepository<Movie> _movieRepository;
		private readonly IGenericRepository<Seat> _seatRepository;
		private readonly IGenericRepository<Showtime> _showtimeRepository;
		private readonly IToastNotification _toastNotification;
		public PaymentsController(IGenericRepository<Ticket> ticketRepository,
			IGenericRepository<Payment> paymentRepository,
			IGenericRepository<Movie> movieRepository,
			IGenericRepository<Seat> seatRepository,
			IGenericRepository<Showtime> showtimeRepository,
			IToastNotification toastNotification)
		{
			_ticketRepository = ticketRepository;
			_paymentRepository = paymentRepository;
			_movieRepository = movieRepository;
			_seatRepository = seatRepository;
			_showtimeRepository = showtimeRepository;
			_toastNotification = toastNotification;
		}

		public async Task<IActionResult> Index()
		{
			var customerId = int.Parse(Request.Cookies["customer_id"]);
			var tickets = _ticketRepository.GetAll().Result.Where(x => x.CustomerId == customerId && x.IsPaid == true).ToList();
			List<TicketVM> ticketList = new();
			foreach (var ticket in tickets.ToList())
			{
				var movie = await _movieRepository.Get(ticket.MovieId);
				var showtime = await _showtimeRepository.Get(ticket.ShowtimeId);
				var seat = await _seatRepository.Get(ticket.SeatId);
				/*	bool isPaid = _paymentRepository.GetAll().Result.Any(p => p.TicketId == ticket.Id);*/
				ticketList.Add(new TicketVM
				{
					Id = ticket.Id,
					CustomerId = customerId,
					MovieId = movie.Id,
					ShowtimeId = showtime.Id,
					MovieName = movie.Title,
					StartTime = showtime.StartTime,
					EndTime = showtime.EndTime,
					SeatNumber = seat.SeatNo,
					Price = ticket.Price,
					IsPaid = ticket.IsPaid
				});
			}
			List<PaymentVM> paymentList = new();
			foreach (var ticket in ticketList)
			{
				if (ticket.IsPaid)
				{
					var payment = _paymentRepository.GetAll().Result.FirstOrDefault(p => p.TicketId == ticket.Id);
					paymentList.Add(new PaymentVM
					{
						PaidTickets = ticketList,
						MovieName = ticket.MovieName,
						Amount = ticket.Price,
						PaymentDate = payment.PaymentDate,
						PaymentMethod = payment.PaymentMethod,
					});

				}
			}

			return View(paymentList);
		}
		public async Task<IActionResult> Create(int ticketId)
		{
			var customerId = int.Parse(Request.Cookies["customer_id"]);
			List<string> paymentMethods = new()
			{
				"MasterCard",
				"Visa",
				"American Express"
			};
			PaymentVM paymentVM = new();
			paymentVM.TicketId = ticketId;
			paymentVM.PaymentMethods = paymentMethods;
			var ticketPaid = await _ticketRepository.Get(ticketId);
			ticketPaid.IsPaid = true;
			_ticketRepository.Update(ticketPaid);
			await _ticketRepository.SaveAsync();
			return View(paymentVM);
		}
		[HttpPost]
		public async Task<IActionResult> Create(PaymentVM model)
		{
			var ticket = await _ticketRepository.Get(model.TicketId);
			Payment payment = new();
			payment.TicketId = model.TicketId;
			payment.PaymentMethod = model.PaymentMethod;
			payment.PaymentDate = DateTime.Now;
			payment.Amount = ticket.Price;
			await _paymentRepository.Add(payment);
			await _paymentRepository.SaveAsync();
			_toastNotification.AddSuccessToastMessage("Ticket paid successfully");
			return RedirectToAction("Index", "Movies");
		}
		/*	public async Task<IActionResult> BasketIndex()
			{
				var customerId = int.Parse(Request.Cookies["customer_id"]);
				var tickets = _ticketRepository.GetAll().Result.Where(x => x.CustomerId == customerId&&x.IsPaid==false).ToList();
				List<TicketVM> list = new();
				foreach (var ticket in tickets.ToList())
				{
					var movie = await _movieRepository.Get(ticket.MovieId);
					var showtime = await _showtimeRepository.Get(ticket.ShowtimeId);
					var seat = await _seatRepository.Get(ticket.SeatId);
					list.Add(new TicketVM
					{
						Id = ticket.Id,
						CustomerId = customerId,
						MovieId = movie.Id,
						ShowtimeId = showtime.Id,
						MovieName = movie.Title,
						StartTime = showtime.StartTime,
						EndTime = showtime.EndTime,
						SeatNumber = seat.SeatNo,
						Price = ticket.Price,

					});
				}
				return View(list);
			}*/
	}
}
