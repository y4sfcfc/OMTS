using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;

namespace OMTS.UI.ViewComponents
{
	public class TopRatedViewComponent:ViewComponent
	{
		private readonly IGenericRepository<Movie> _movieRepository;
		private readonly IGenericRepository<Review> _reviewRepository;

		public TopRatedViewComponent(IGenericRepository<Movie> movieRepository,
			IGenericRepository<Review> reviewRepository)
		{
			_movieRepository = movieRepository;
			_reviewRepository = reviewRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var movies = await GetItemsAsync();

			
			return View(movies);
		}
		private async Task<List<MovieVM>> GetItemsAsync()
		{
			var movies = await _movieRepository.GetAll(query => query.Include(m => m.Reviews));
			List<MovieVM> list = new();
			foreach (var movie in movies)
			{
				float avr = 0;
				foreach (var review in movie.Reviews)
				{
					avr += review.Rating;
				}
				avr = avr / movie.Reviews.Count();
				list.Add(new MovieVM { Title=movie.Title,Average=avr });
			}
			return list.OrderByDescending(x => x.Average).ToList();
		}
	}
}
