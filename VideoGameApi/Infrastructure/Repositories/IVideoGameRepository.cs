using VideoGameApi.Domain;

namespace VideoGameApi.Infrastructure.Repositories
{
    public interface IVideoGameRepository
    {
        Task<VideoGame> GetByIdAsync(int id);
        Task<List<VideoGame>> GetAllAsync();
        Task AddAsync(VideoGame game);
        Task UpdateAsync(VideoGame game);
        Task<bool> DeleteAsync(int id);
        Task<List<VideoGame>> SearchByNameAsync(string keyword);
        Task<IQueryable<VideoGame>> GetAllWithDetailsAsQueryableAsync();
        Task<bool> UserVideoGameExist(Guid UserId,int gameid);
        Task AddToUserVideoGame(UserVideoGame userVideoGame);
    }
}
