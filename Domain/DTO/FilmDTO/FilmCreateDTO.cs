namespace Domain.DTO.FilmDTO
{
    public class FilmCreateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public List<Guid> ActorIds { get; set; } = new();
        public List<Guid> ReviewIds { get; set; } = new();
    }
}
