using Domain.DTO.ReviewDTO;

namespace BusinessLogic.Interfaces
{
    public interface IReviewService
    {
        Task<Guid?> CreateReview(ReviewCreateDTO createDTO);
        Task<Guid?> DeleteReview(Guid id);
        Task<List<ReviewReadDTO>> GetAllReviews();
        Task<ReviewReadDTO?> GetReviewById(Guid id);
        Task<bool?> UpdateReview(ReviewUpdateDTO reviewDTO);
    }
}