using VideoGameApi.Application.DTO.GameDto;

namespace VideoGameApi.Application.DTO.Genre
{
    public class GenreDto
    {
        public int GenreId { get; set; }
        
        public string Name { get; set; }

        public ICollection<VideoGameDto> VideoGames { get; set; }
    }
}
