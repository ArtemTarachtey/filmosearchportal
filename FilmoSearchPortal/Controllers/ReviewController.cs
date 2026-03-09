using BusinessLogic.Interfaces;
using Domain.DTO.ReviewDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmoSearchPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewController> _logger;
        public ReviewController(IReviewService service, ILogger<ReviewController> logger)
        {
            _reviewService = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            _logger.LogInformation("Запрашиваю все отзывы у бд...");
            var reviews = await _reviewService.GetAllReviews();
            if (reviews == null)
            {
                _logger.LogWarning("Ошибка. Отзывы не найдены в бд!");
                return NotFound("Reviews not found!");
            }
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(Guid id)
        {
            _logger.LogInformation("Запрашиваю отзыв с id: {id}...", id);
            var review = await _reviewService.GetReviewById(id);
            if (review == null)
            {
                _logger.LogWarning("Ошибка. Отзыв с id: {ReviewId} не найден", id);
                return NotFound("Review with this Id is not found!");
            }
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDTO reviewDTO)
        {
            _logger.LogInformation("Создаю отзыв: Title: {Title}, Text: {Text}, Stars: {Stars}, FilmId: {FilmId}",
                reviewDTO.Title,
                reviewDTO.Text,
                reviewDTO.Stars,
                reviewDTO.FilmId);

            var reviewId = await _reviewService.CreateReview(reviewDTO);
            if (reviewId == null)
            {
                _logger.LogWarning("Ошибка. Отзыв не был создан!");
                return BadRequest("Review was not created!");
            }
            return Ok(reviewId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewUpdateDTO reviewDTO)
        {
            _logger.LogInformation("Обновляю отзыв: Id: {reviewDTO.Id} -- Title: {reviewDTO.Title} Text: {reviewDTO.Text} Stars: {reviewDTO.Stars} FilmId: {reviewDTO.FilmId}...",
                reviewDTO.Id,
                reviewDTO.Title,
                reviewDTO.Text,
                reviewDTO.Stars,
                reviewDTO.FilmId);
            var isUpdated = await _reviewService.UpdateReview(reviewDTO);
            if (isUpdated == null)
            {
                _logger.LogWarning("Ошибка. Отзыв не был обновлен!");
                return NotFound("Review with this Id is not found!");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            _logger.LogInformation("Удаляю отзыв: Id: {id}...", id);
            var deletedId = await _reviewService.DeleteReview(id);
            if (deletedId == null)
            {
                _logger.LogWarning("Ошибка. Отзыв не был удалён!");
                return NotFound("Review with this Id is not found!");
            }
            return Ok(deletedId);
        }

    }
}
