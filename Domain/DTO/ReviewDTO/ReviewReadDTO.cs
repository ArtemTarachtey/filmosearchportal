using Domain.DTO.ActorDTO;

namespace Domain.DTO.ReviewDTO
{
    public class ReviewReadDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Stars { get; set; }
        public FilmShortDTO Film { get; set; } = new();
    }
}
