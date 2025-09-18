using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Domain
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Navigation Property
        public ICollection<VideoGame> VideoGames { get; set; }
    }
}
