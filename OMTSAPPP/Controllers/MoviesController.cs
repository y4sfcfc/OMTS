using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;

namespace OMTS.UI.Controllers
{
	public class MoviesController : Controller
	{
		private readonly IGenericRepository<Movie> _movieRepository;
		private readonly IToastNotification _toastNotification;

		public MoviesController(IGenericRepository<Movie> movieRepository, IToastNotification toastNotification)
		{
			_movieRepository = movieRepository;
			_toastNotification = toastNotification;
		}

		public async Task<IActionResult> Index(int customerId)
		{
			var movies = await _movieRepository.GetAll();
			List<MovieVM> list = new List<MovieVM>();
			foreach (var movie in movies)
			{
				list.Add(new MovieVM
				{
					Id = movie.Id,
					Title = movie.Title,
					Duration = movie.Duration,
					Genre = movie.Genre,
					Director = movie.Director,
					Thumbnail = movie.Thumbnail,
					ReleaseDate = movie.ReleaseDate,
					CustomerId = customerId

				});
			}
			_toastNotification.AddErrorToastMessage("sc bshdc js");
			return View(list);
		}

		public async Task<IActionResult> Details(int customerId, int movieId)
		{

			return RedirectToAction("Index", "Showtimes", new { customerId = customerId, movieId = movieId });
		}
		public async Task<IActionResult> UpcomingMovies()
		{
			var movies =await _movieRepository.GetAll();
			List<MovieVM> list = new();
            foreach (var movie in movies)
            {
				if (DateTime.Now<movie.ReleaseDate)
				{
					list.Add(new MovieVM
					{
						ReleaseDate = movie.ReleaseDate,
						Title = movie.Title,
					});
				}   
            }
            return PartialView("_UpcomingMovies",list);
		}
	}
}
