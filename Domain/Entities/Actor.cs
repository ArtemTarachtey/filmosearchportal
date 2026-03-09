using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Range(0, 200, ErrorMessage = "Invalid age!")]
        public int Age { get; set; }

        [Range(0f, 10f, ErrorMessage = "Invalid rating!")]
        public float Rating {  get; set; }
        public ICollection<Film> Films { get; set; } = new List<Film>();

    }
}
