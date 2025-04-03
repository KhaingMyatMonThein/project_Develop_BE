using PD.Models;

namespace PD.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsAsync();
        Task AddReviewAsync(Review review);
    }
}
