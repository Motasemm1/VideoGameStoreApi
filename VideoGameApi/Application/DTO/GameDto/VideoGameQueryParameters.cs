using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.GameDto
{
    public class VideoGameQueryParameters
    {
        // Filtering
        public int? GenreId { get; set; }
        public int? PublisherId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Title { get; set; }

        // Sorting
        public string? SortBy { get; set; }  // e.g. "price", "releaseDate"
        public bool IsDescending { get; set; } = false;

        // Pagination
        [Required]
        public int PageNumber { get; set; } = 1;
        [Required]
        public int PageSize { get; set; } = 10;
    }
}
