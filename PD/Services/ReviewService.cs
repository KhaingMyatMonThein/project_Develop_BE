using PD.Models;
using PD.Repositories;

namespace PD.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync()
        {
            return await _reviewRepository.GetReviewsAsync();
        }

        public async Task AddReviewAsync(Review review)
        {
            await _reviewRepository.AddReviewAsync(review);
        }
    }
}
