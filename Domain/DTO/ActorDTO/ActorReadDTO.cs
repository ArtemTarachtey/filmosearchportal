namespace Domain.DTO.ActorDTO
{
    public class FilmShortDTO
    {
        public Guid FilmId { get; set; }
        public string FilmTitle { get; set; } = string.Empty;
    }
    public class ActorReadDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public float Rating { get; set; }
        public List<FilmShortDTO> Films { get; set; } = new();
    }
}
