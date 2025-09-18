using VideoGameApi.Application.DTO.Genre;
using VideoGameApi.Domain;

namespace VideoGameApi.Infrastructure.Repositories
{
    public interface IGenreRepository
    {
        Task<Genre?> GetByIdAsync(int id);
        Task<IEnumerable<Genre?>> GetAllAsync();
        Task AddAsync(Genre genre);
        Task UpdateAsync(Genre genre);
        Task DeleteAsync(Genre genre);
        Task<bool> ExistsAsync(int id);
    }
}
