using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;

namespace OMTS.UI.Controllers
{
	public class SeatController : Controller
	{
		private readonly IGenericRepository<Seat> _seatRepository;
		private readonly IGenericRepository<Showtime> _showtimeRepository;

		public SeatController(IGenericRepository<Seat> seatRepository,
			IGenericRepository<Showtime> showtimeRepository,
			IToastNotification toastNotification)
		{
			_seatRepository = seatRepository;
			_showtimeRepository = showtimeRepository;
		}

		public async Task<IActionResult> Index(int showtimeId,int movieId)
		{
			int customerId = int.Parse(Request.Cookies["customer_id"]);
			var showTime = (await _showtimeRepository.GetAll(
			 query => query.Include(s => s.Tickets).ThenInclude(t => t.Seat)
		 )).FirstOrDefault(s => s.Id == showtimeId);
			var seats = _seatRepository.GetAll().Result.Where(x => x.CinemaHallId == showTime.CinemaHallId);
			IEnumerable<SeatVM> list = seats.Select(seat => new SeatVM
			{
				SeatNo = seat.SeatNo,
				SeatId=seat.Id,
				IsBooked = showTime.Tickets.Any(t => t.SeatId == seat.Id),
				ShowtimeId = showtimeId,
			});
			SeatSelectVM seatSelectVM = new();
			seatSelectVM.CustomerId= customerId;
			seatSelectVM.ShowtimeId = showtimeId;
			seatSelectVM.MovieId = movieId;
			seatSelectVM.Seats = list.ToList();
			return View(seatSelectVM);
		}
	}
}
