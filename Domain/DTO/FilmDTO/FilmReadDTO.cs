namespace Domain.DTO.FilmDTO
{
    public class ActorShortDTO
    {
        public Guid actorId { get; set; }
        public string actorName { get; set; } = string.Empty;
    }

    public class ReviewShortDTO
    {
        public Guid reviewId { get; set; }
        public string reviewName { get; set;} = string.Empty;
    }
    public class FilmReadDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public List<ActorShortDTO> Actors { get; set; } = new();
        public List<ReviewShortDTO> Reviews { get; set; } = new();
    }
}
