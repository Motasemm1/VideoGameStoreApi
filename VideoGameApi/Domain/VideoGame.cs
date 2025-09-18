using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoGameApi.Domain
{
    public class VideoGame
    {
        [Key]
        public int GameId { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime? ReleaseDate { get; set; }
        [Required]
        public int GenreId { get; set; }  // FK
        [Required]
        public int PublisherId { get; set; }  // FK
        [MaxLength(500)]
        public string? ImageUrl { get; set; }
        // Navigation properties
        public Genre Genre { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<UserVideoGame> UserVideoGames { get; set; }
    }
}
