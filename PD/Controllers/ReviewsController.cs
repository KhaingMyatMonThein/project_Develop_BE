using Microsoft.AspNetCore.Mvc;
using PD.Dto;
using PD.Models;
using PD.Services;

namespace PD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewsController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await _reviewService.GetReviewsAsync();
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(ReviewDto reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = new Review
            {
                Name = reviewDto.Name,
                Email = reviewDto.Email,
                Rating = reviewDto.Rating,
                ReviewText = reviewDto.ReviewText
            };

            await _reviewService.AddReviewAsync(review);
            return CreatedAtAction(nameof(GetReviews), new { id = review.Id }, review);
        }

    }
}
