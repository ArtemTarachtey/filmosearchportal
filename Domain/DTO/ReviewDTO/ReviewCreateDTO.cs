namespace Domain.DTO.ReviewDTO
{
    public class ReviewCreateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Stars { get; set; }
        public Guid FilmId { get; set; }
    }
}
