using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;
using System.Net.Sockets;

namespace OMTS.UI.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<Showtime> _showtimeRepository;
        private readonly IGenericRepository<Seat> _seatRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IToastNotification _toastNotification;
		public TicketsController(IGenericRepository<Ticket> ticketRepository,
			IGenericRepository<Movie> movieRepository,
			IGenericRepository<Customer> customerRepository,
			IGenericRepository<Showtime> showtimeRepository,
			IGenericRepository<Seat> seatRepository,
			IGenericRepository<Payment> paymentRepository,
			IToastNotification toastNotification)
		{
			_ticketRepository = ticketRepository;
			_movieRepository = movieRepository;
			_customerRepository = customerRepository;
			_showtimeRepository = showtimeRepository;
			_seatRepository = seatRepository;
			_paymentRepository = paymentRepository;
			_toastNotification = toastNotification;
		}

		public async Task<IActionResult> Index(int customerId, int movieId)
        {
            var tickets = _ticketRepository.GetAll().Result.Where(x => x.MovieId == movieId);
            List<TicketVM> list = new();

            foreach (var ticket in tickets.ToList())
            {
                var customer = await _customerRepository.Get(customerId);
                var movie = await _movieRepository.Get(movieId);
                var showtime = await _showtimeRepository.Get(ticket.ShowtimeId);

                list.Add(new TicketVM
                {
                    Id = ticket.Id,
                    CustomerId = customerId,
                    MovieId = movie.Id,
                    ShowtimeId = showtime.Id,
                    MovieName = movie.Title,
                    StartTime = showtime.StartTime,
                    EndTime = showtime.EndTime,
                    Price = ticket.Price,

                });
            }

            return View(list);
        }
        /*	public async Task<IActionResult> Create(int? customerId, int showtimeId, int? movieId, int seatNo)
            {
                var customer_id = int.Parse(Request.Cookies["customer_id"]);
                var ticketSeats = await _ticketRepository.GetAll();
                var availableSeats = _ticketRepository.GetAll().Result.Where(x => x.ShowtimeId == showtimeId).ToList();

                SeatSelectVM seatSelectVM = new();
                seatSelectVM.CustomerId = customer_id;
                seatSelectVM.ShowtimeId = showtimeId;
                seatSelectVM.MovieId = movieId ?? 0;

                return View(seatSelectVM);
            }*/
        [HttpPost]
        public async Task<IActionResult> Create(SeatSelectVM model)
        {
            Ticket ticket = new();
			bool isPaid = _paymentRepository.GetAll().Result.Any(p => p.TicketId == ticket.Id);
			ticket.MovieId = model.MovieId;
            ticket.CustomerId = model.CustomerId;
            ticket.ShowtimeId = model.ShowtimeId;
            ticket.SeatId = model.SeatNo;
            ticket.Price = 5;
            ticket.IsPaid = isPaid;
            await _ticketRepository.Add(ticket);
            await _ticketRepository.SaveAsync();
            _toastNotification.AddSuccessToastMessage("Ticket Added to basket");
            return RedirectToAction("Index", "Movies");
        }
        public async Task<IActionResult> BasketIndex()
        {
            var customerId = int.Parse(Request.Cookies["customer_id"]);
			var tickets = _ticketRepository.GetAll().Result.Where(x => x.CustomerId == customerId && x.IsPaid == false).ToList();
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
                    IsPaid = ticket.IsPaid
                }) ;
            }
            return View(list);
        }
    }
}
