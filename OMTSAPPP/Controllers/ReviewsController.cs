using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMTS.DAL.Models;
using OMTS.DAL.Repository.Interfaces;
using OMTS.UI.Models;

namespace OMTS.UI.Controllers
{
	public class ReviewsController : Controller
	{
		private readonly IGenericRepository<Review> _reviewRepository;

		public ReviewsController(IGenericRepository<Review> reviewRepository)
		{
			_reviewRepository = reviewRepository;
		}

		public async Task<IActionResult> Create(int? customerId, int? movieId)
		{
			int customerid = int.Parse(Request.Cookies["customer_id"]);
			var reviews = _reviewRepository.GetAll(query => query.Include(x => x.Customer)).Result.Where(r => r.MovieId == movieId);
			ReviewVM reviewVM = new();
			reviewVM.CustomerId = customerid;
			reviewVM.MovieId = movieId ?? 0;
			reviewVM.Reviews = reviews.ToList();
			return View(reviewVM);
		}
		[HttpPost]
		public async Task<IActionResult> Create(ReviewVM model)
		{
			int customerid = int.Parse(Request.Cookies["customer_id"]);
			Review review = new Review();
			review.MovieId = model.MovieId;
			review.CustomerId = customerid;
			review.Rating = model.Rating;
			review.Comment = model.Comment;
			await _reviewRepository.Add(review);
			await _reviewRepository.SaveAsync();
			return RedirectToAction("Index", "Movies");
		}
		public async Task<IActionResult> Delete(int reviewId)
		{
			int customerid = int.Parse(Request.Cookies["customer_id"]);
			var review = await _reviewRepository.Get(reviewId);
			if (review.CustomerId == customerid)
			{
				_reviewRepository.Delete(reviewId);
			}
			return RedirectToAction("Index", "Movies");
		}
	}
}
