using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Domain
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // Navigation Property
        public ICollection<VideoGame> VideoGames { get; set; }
    }
}
