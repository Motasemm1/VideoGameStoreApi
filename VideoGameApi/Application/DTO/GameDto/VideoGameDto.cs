namespace VideoGameApi.Application.DTO.GameDto
{
    public class VideoGameDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } 
        public string? GenreName { get; set; }    
        public string? PublisherName { get; set; }
        public decimal Price { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? ImageUrl { get; set; }
    }
}

