using VideoGameApi.Application.DTO.GameDto;

namespace VideoGameApi.Application.Intefaces
{
    public interface IVideoGameService
    {
        Task <List<VideoGameDto>> GetAllAsync();
        Task<int> AddVideoGameAsync(CreateGameDto dto);

        Task<bool> UpdateGameAsync(int id, UpdateVideoGameDto dto);

        Task<bool> DeleteAsync(int id);

        Task<VideoGameDto> GetByIdAsync(int id);

       
        Task<List<VideoGameDto>> SearchByName(string GameName);
        Task<PageResultDto<VideoGameDto>> GetVideoGamesSortedAsync(VideoGameQueryParameters queryParams);
    }
}
