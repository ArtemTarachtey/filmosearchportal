using BusinessLogic.Interfaces;
using DataAccess.Context;
using Domain.DTO.ActorDTO;
using Domain.DTO.ReviewDTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _dbContext;
        public ReviewService(AppDbContext context) => _dbContext = context;
        public async Task<ReviewReadDTO?> GetReviewById(Guid id)
        {
            return await _dbContext.Reviews.Where(r => r.Id == id).Select(r => new ReviewReadDTO { Id = r.Id, Title = r.Title, Text = r.Text, Stars = r.Stars, Film = new FilmShortDTO { FilmId = r.Film.Id, FilmTitle = r.Film.Title } }).FirstOrDefaultAsync();
        }
        public async Task<List<ReviewReadDTO>> GetAllReviews()
        {
            return await _dbContext.Reviews.AsNoTracking().Select(r => new ReviewReadDTO { Id = r.Id, Title = r.Title, Text = r.Text, Stars = r.Stars, Film = new FilmShortDTO { FilmId = r.Film.Id, FilmTitle = r.Film.Title} }).ToListAsync();
        }
        public async Task<Guid?> CreateReview(ReviewCreateDTO createDTO)
        {
            var review = new Review { Id = Guid.NewGuid(), Title = createDTO.Title, Text = createDTO.Text, Stars = createDTO.Stars, FilmId = createDTO.FilmId };
            try
            {
                await _dbContext.Reviews.AddAsync(review);
                await _dbContext.SaveChangesAsync();
                return review.Id;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<bool?> UpdateReview(ReviewUpdateDTO reviewDTO)
        {
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewDTO.Id);
            if (review == null) return null;
            review.Title = reviewDTO.Title;
            review.Text = reviewDTO.Text;
            review.Stars = reviewDTO.Stars;
            review.FilmId = reviewDTO.FilmId;
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Guid?> DeleteReview(Guid id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);
            if (review == null) return null;
            try
            {
                _dbContext.Remove(review);
                await _dbContext.SaveChangesAsync();
                return review.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
