using VideoGameApi.Application.DTO;
using VideoGameApi.Application.DTO.Genre;

namespace VideoGameApi.Application.Intefaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto?>> GetAllGenres();
        Task <GenreDto?> GetGenreById(int? id);
        Task <int> AddNewGenre(CreateGenreDto newGenre);
        Task <bool>EditGenre(int id, UpdateGenreDto newGenre);
        Task <Result>DeleteGenreById(int id);
    }
}
