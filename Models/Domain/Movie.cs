using System.ComponentModel.DataAnnotations;

namespace MovieStore.Models.Domain
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? ReleasedYear { get; set; }

        [Required]
        public string? MovieImage { get; set; }

        [Required]
        public string? Cast { get; set; }

        [Required]
        public string? Director { get; set; }
    }
}
