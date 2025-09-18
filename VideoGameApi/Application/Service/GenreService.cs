using Microsoft.EntityFrameworkCore;
using VideoGameApi.Application.DTO;
using VideoGameApi.Application.DTO.Genre;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Application.DTO.GameDto;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;
using VideoGameApi.Infrastructure.Repositories;

namespace VideoGameApi.Application.Service
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<int> AddNewGenre(CreateGenreDto newGenre)
        {
            var genre = new Genre
            {
                Name = newGenre.Name
            };
            await _genreRepository.AddAsync(genre);
            return genre.GenreId;
        }

        public async Task<Result> DeleteGenreById(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);

            if (genre == null)
                return new Result { Success = false, Message = "Genre not found" };
            if (genre.VideoGames.Any())
                return new Result { Success = false, Message = "Cannot delete genre with associated video games" };


            await _genreRepository.DeleteAsync(genre);
            return new Result { Success = true, Message = "Genre deleted successfully" };
        }

        public async Task<bool> EditGenre(int id, UpdateGenreDto newGenre)
        {
            var oldGenre = await _genreRepository.GetByIdAsync(id);
            if (oldGenre == null)
                return false;
            oldGenre.Name = newGenre.Name;
            await _genreRepository.UpdateAsync(oldGenre);
            return true;
        }

        public async Task<IEnumerable<GenreDto?>> GetAllGenres()
        {
            var genres = await _genreRepository.GetAllAsync();
            if (genres == null)
                return null;
            var list = genres.Select(g => new GenreDto
            {
                GenreId = g.GenreId,
                Name = g.Name,
                VideoGames = g.VideoGames.Select(v => new VideoGameDto
                {
                    Id = v.GameId,
                    Description = v.Description,
                    GenreName = g.Name,
                    Price = v.Price,
                    PublisherName = v.Publisher?.Name,
                    ReleaseDate = v.ReleaseDate,
                    Title = v.Title
                }).ToList()
            }).ToList();
            return list;
        }

        public async Task<GenreDto?> GetGenreById(int? id)
        {
            if (id == null)
                return null;

            var genre = await _genreRepository.GetByIdAsync(id.Value);
            if (genre == null)
                return null;

            return new GenreDto
            {
                GenreId = genre.GenreId,
                Name = genre.Name,
                VideoGames = genre.VideoGames.Select(v => new VideoGameDto
                {
                    Id = v.GameId,
                    Description = v.Description,
                    GenreName = genre.Name,
                    Price = v.Price,
                    PublisherName = v.Publisher.Name,
                    ReleaseDate = v.ReleaseDate,
                    Title = v.Title
                }).ToList()

            };
        }

    }
}
