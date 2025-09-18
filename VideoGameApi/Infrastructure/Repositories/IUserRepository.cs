using VideoGameApi.Application.DTO.UserDto;
using VideoGameApi.Domain;

namespace VideoGameApi.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid userId);
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> ExistsByUsernameAsync(string username);
        Task Savechanges();
        Task<User?> GetByIdWithOrdersAndGamesAsync(Guid id);
    }
}
