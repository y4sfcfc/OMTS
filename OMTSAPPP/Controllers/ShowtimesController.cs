using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;

namespace OMTS.UI.Controllers
{
	public class ShowtimesController : Controller
	{
		private readonly IGenericRepository<Showtime> _showtimeRepository;
		private readonly IGenericRepository<Movie> _movieRepository;
		private readonly IGenericRepository<CinemaHall> _cinemaHallRepository;

		public ShowtimesController(IGenericRepository<Showtime> showtimeRepository, IGenericRepository<Movie> movieRepository, IGenericRepository<CinemaHall> cinemaHallRepository, IToastNotification toastNotification)
		{
			_showtimeRepository = showtimeRepository;
			_movieRepository = movieRepository;
			_cinemaHallRepository = cinemaHallRepository;
		}

		public async Task<IActionResult> Index(int customerId, int movieId)
		{

			List<ShowtimeVM> list = new List<ShowtimeVM>();
			var showtimes = _showtimeRepository.GetAll().Result.Where(x => x.MovieId == movieId);
			var movie = await _movieRepository.Get(movieId);
			foreach (var showtime in showtimes.ToList())
			{
				var cinemaHall = await _cinemaHallRepository.Get(showtime.CinemaHallId);
				list.Add(new ShowtimeVM
				{
					Id = showtime.Id,
					MovieId = movieId,
					MovieTitle = movie.Title,
					CinemaHallId = showtime.CinemaHallId,
					CinemaHallName = cinemaHall.Name,
					CustomerId = customerId,
					StartTime = showtime.StartTime,
					EndTime = showtime.EndTime,
				});
			}
			return View(list);
		}

	}
}
