using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        [Range(0, 5, ErrorMessage = "Invalid stars!")]
        public int Stars {  get; set; }
        public Guid FilmId { get; set; }
        public Film Film { get; set; } = null!;
    }
}
