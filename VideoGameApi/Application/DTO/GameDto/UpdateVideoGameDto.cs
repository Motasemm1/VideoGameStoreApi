namespace VideoGameApi.Application.DTO.GameDto
{
    public class UpdateVideoGameDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int GenreId { get; set; }
        public int PublisherId { get; set; }
        public IFormFile? ImageUrl { get; set; } 
    }
}
