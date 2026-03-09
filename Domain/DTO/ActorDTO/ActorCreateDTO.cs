namespace Domain.DTO.ActorDTO
{
    public class ActorCreateDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public float Rating { get; set; }
        public List<Guid> FilmIds { get; set; } = new();
    }
}
