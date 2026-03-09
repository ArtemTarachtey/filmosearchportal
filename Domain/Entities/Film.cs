using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Film
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Range(1, 2000, ErrorMessage = "Invalid duration!")]
        public int Duration { get; set; }
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}
