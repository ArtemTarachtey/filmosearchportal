using BusinessLogic.Interfaces;
using Domain.DTO.ActorDTO;
using Domain.DTO.ReviewDTO;
using FilmoSearchPortal.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FilmoSearchPortal.Tests.UnitTests.Controllers
{
    public class ReviewControllerTest
    {
        private readonly Mock<IReviewService> _serviceMock;
        private readonly Mock<ILogger<ReviewController>> _loggerMock;
        private readonly ReviewController _reviewController;
        public ReviewControllerTest()
        {
            _serviceMock = new Mock<IReviewService>();
            _loggerMock = new Mock<ILogger<ReviewController>>();
            _reviewController = new ReviewController(_serviceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task GetReview_PositiveTest()
        {
            var id = Guid.NewGuid();

            var review = new ReviewReadDTO { Id = id, Title = "Test Title", Text = "Test text", Stars = 5, Film = new FilmShortDTO { FilmId = Guid.NewGuid(), FilmTitle = "Test Film Title" } };

            _serviceMock.Setup(s => s.GetReviewById(id)).ReturnsAsync(review);

            var r = await _reviewController.GetReview(id);

            var isOk = Assert.IsType<OkObjectResult>(r);
            var returnedReview = Assert.IsType<ReviewReadDTO>(isOk.Value);
            Assert.Equal(id, returnedReview.Id);
        }
        [Fact]
        public async Task GetReview_NegativeTest()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.GetReviewById(id)).ReturnsAsync((ReviewReadDTO)null!);

            var r = await _reviewController.GetReview(id);

            var notFound = Assert.IsType<NotFoundObjectResult>(r);
            Assert.Equal("Review with this Id is not found!", notFound.Value);
        }


        [Fact]

        public async Task GetAllReviews_PositiveTest()
        {
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var reviews = new List<ReviewReadDTO>{ new ReviewReadDTO { Id = id, Title = "Test Title", Text = "Test text", Stars = 5, Film = new FilmShortDTO { FilmId = Guid.NewGuid(), FilmTitle = "Test Film Title" } }, new ReviewReadDTO { Id = id2, Title = "Test Title2", Text = "Test text2", Stars = 2, Film = new FilmShortDTO { FilmId = Guid.NewGuid(), FilmTitle = "Test Film Title2" } }};

            _serviceMock.Setup(s => s.GetAllReviews()).ReturnsAsync(reviews);

            var r = await _reviewController.GetAllReviews();

            var isOk = Assert.IsType<OkObjectResult>(r);
            var returnedReviews = Assert.IsAssignableFrom<IEnumerable<ReviewReadDTO>>(isOk.Value);
            Assert.Equal(2, returnedReviews.Count());
        }

        [Fact]

        public async Task GetAllReviews_NegativeTest()
        {

            _serviceMock.Setup(s => s.GetAllReviews()).ReturnsAsync((List<ReviewReadDTO>?)null);

            var r = await _reviewController.GetAllReviews();

            var notFound = Assert.IsType<NotFoundObjectResult>(r);
            Assert.Equal("Reviews not found!", notFound.Value);
        }

        [Fact]
        public async Task CreateReview_PositiveTest()
        {
            var review = new ReviewCreateDTO { Title = "Test Title", Text = "Test text", Stars = 5, FilmId = Guid.NewGuid() };

            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.CreateReview(review)).ReturnsAsync(id);

            var r = await _reviewController.CreateReview(review);

            var result = Assert.IsType<OkObjectResult>(r);
            Assert.Equal(id, result.Value);

        }
        [Fact]
        public async Task CreateReview_NegativeTest()
        {
            var review = new ReviewCreateDTO { Title = "Test Title", Text = "Test text", Stars = 5, FilmId = Guid.NewGuid() };

            var id = Guid.NewGuid();
            _serviceMock.Setup(s => s.CreateReview(review)).ReturnsAsync((Guid?)null);

            var r = await _reviewController.CreateReview(review);

            var result = Assert.IsType<BadRequestObjectResult>(r);
            Assert.Equal("Review was not created!", result.Value);
        }

        [Fact]
        public async Task UpdateReview_PositiveTest()
        {
            var id = Guid.NewGuid();
            var review = new ReviewUpdateDTO { Id=id, Title = "Test Title", Text = "Test text", Stars = 5, FilmId = Guid.NewGuid() };

            _serviceMock.Setup(s => s.UpdateReview(review)).ReturnsAsync(true);

            var r = await _reviewController.UpdateReview(review);

            Assert.IsType<NoContentResult>(r);
        }

        [Fact]
        public async Task UpdateReview_NegativeTest()
        {
            var id = Guid.NewGuid();
            var review = new ReviewUpdateDTO { Id = id, Title = "Test Title", Text = "Test text", Stars = 5, FilmId = Guid.NewGuid() };

            _serviceMock.Setup(s => s.UpdateReview(review)).ReturnsAsync((bool?)null);

            var r = await _reviewController.UpdateReview(review);
            var result = Assert.IsType<NotFoundObjectResult>(r);
            Assert.Equal("Review with this Id is not found!", result.Value);
        }
        [Fact]

        public async Task ReviewDelete_PositiveTest()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.DeleteReview(id)).ReturnsAsync(id);

            var r = await _reviewController.DeleteReview(id);

            var isOk = Assert.IsType<OkObjectResult>(r);
            Assert.Equal(id, isOk.Value);
        }
        [Fact]

        public async Task ReviewDelete_NegativeTest()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.DeleteReview(id)).ReturnsAsync((Guid?)null);

            var r = await _reviewController.DeleteReview(id);

            var isOk = Assert.IsType<NotFoundObjectResult>(r);
            Assert.Equal("Review with this Id is not found!", isOk.Value);
        }
    }
}
