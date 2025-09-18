using VideoGameApi.Application.DTO.GameDto;

namespace VideoGameApi.Application.DTO.Publisher
{
    public class PublisherDto
    {
        public int PublisherId { get; set; }

        public string Name { get; set; }

        public ICollection<VideoGameDto>? VideoGames { get; set; }
    }
}
