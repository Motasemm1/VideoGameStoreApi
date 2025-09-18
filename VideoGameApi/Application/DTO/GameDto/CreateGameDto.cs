using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.GameDto
{
    public class CreateGameDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public int PublisherId { get; set; }
      
        public IFormFile Image { get; set; }
    }
}
