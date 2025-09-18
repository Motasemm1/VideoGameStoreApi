using Microsoft.EntityFrameworkCore;
using VideoGameApi.Application.DTO.GameDto;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;
using VideoGameApi.Infrastructure.Repositories;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace VideoGameApi.Application.Service
{
    public class VideoGameService : IVideoGameService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVideoGameRepository _videoGameRepository   ;

        public VideoGameService(IPublisherRepository publisherRepository, IHttpContextAccessor httpContextAccessor , IVideoGameRepository videoGameRepository, IGenreRepository genreRepository)
        {
           _publisherRepository = publisherRepository;
            _httpContextAccessor = httpContextAccessor;
            _videoGameRepository = videoGameRepository; ;
            _genreRepository = genreRepository;
        }
        public async Task<VideoGameDto> GetByIdAsync(int id)
        {
            var game = await _videoGameRepository.GetByIdAsync(id);
            if (game == null)
                return null;

            return new VideoGameDto
            {
                Id = game.GameId,
                Title = game.Title,
                Description = game.Description,
                GenreName = game.Genre.Name,
                PublisherName = game.Publisher.Name,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                ImageUrl = game.ImageUrl
            };

        }


        public async Task<int> AddVideoGameAsync(CreateGameDto dto)
        {
            string? imagePath = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/games");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                var savePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                var request = _httpContextAccessor.HttpContext!.Request;
                imagePath = $"{request.Scheme}://{request.Host}/images/games/{fileName}";
            }
            //todo after genreRepository
            var genreExists = await _genreRepository.ExistsAsync(dto.GenreId);
            //todo after publisher repository
            var publisherExists = await _publisherRepository.ExistsAsync(dto.PublisherId);

            if (!genreExists || !publisherExists)
                throw new ArgumentException("Invalid Genre or Publisher");


            var game = new VideoGame
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                ReleaseDate = dto.ReleaseDate,
                GenreId = dto.GenreId,
                PublisherId = dto.PublisherId,
                ImageUrl = imagePath,
            };
            await _videoGameRepository.AddAsync(game);

            return game.GameId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _videoGameRepository.GetByIdAsync(id);
            if (game == null) return false; // اللعبة مش موجودة

            await _videoGameRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> UpdateGameAsync(int id, UpdateVideoGameDto dto)
        {
            var game = await _videoGameRepository.GetByIdAsync(id);
            if (game == null) return false;

            if (dto.ImageUrl == null || dto.ImageUrl.Length == 0)
                throw new ArgumentException("Image file is required for PUT updates.");

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/games");
            Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageUrl.FileName);
            var savePath = Path.Combine(folder, fileName);
            using (var fs = new FileStream(savePath, FileMode.Create))
                await dto.ImageUrl.CopyToAsync(fs);
            // احذف القديمة لو موجودة
            if (!string.IsNullOrEmpty(game.ImageUrl))
            {
                var oldAbs = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", game.ImageUrl.TrimStart('/'));
                if (File.Exists(oldAbs)) File.Delete(oldAbs);
            }

            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var imageUrl = $"{baseUrl}/images/games/{fileName}";

            var genreExists = await _genreRepository.ExistsAsync(dto.GenreId);
            var publisherExists = await _publisherRepository.ExistsAsync(dto.PublisherId);

            if (!genreExists || !publisherExists)
                throw new ArgumentException("Invalid Genre or Publisher");

            game.Title = dto.Title;
            game.Description = dto.Description;
            game.Price = dto.Price;
            game.ReleaseDate = dto.ReleaseDate;
            game.GenreId = dto.GenreId;
            game.PublisherId = dto.PublisherId;
            game.ImageUrl = imageUrl;
            await _videoGameRepository.UpdateAsync(game);
            return true;
        }

        public async Task<List<VideoGameDto>> SearchByName(string GameName)
        {
            if (string.IsNullOrWhiteSpace(GameName))
                return new List<VideoGameDto>();

            var game = await _videoGameRepository.SearchByNameAsync(GameName);


            var result = game.Select(v => new VideoGameDto
            {
                Id = v.GameId,
                Title = v.Title,
                Description = v.Description,
                GenreName = v.Genre.Name,
                PublisherName = v.Publisher.Name,
                ReleaseDate = v.ReleaseDate,
                Price = v.Price
            }).ToList();
            return result;
        }

        public async Task<List<VideoGameDto>> GetAllAsync()
        {
            var games = await _videoGameRepository.GetAllAsync();
            if (games == null)
                return null;

            return games
            .Select(v => new VideoGameDto
            {
                Id = v.GameId,
                Title = v.Title,
                Description = v.Description,
                GenreName = v.Genre.Name,
                PublisherName = v.Publisher.Name,
                Price = v.Price,
                ReleaseDate = v.ReleaseDate
            })
            .ToList();
        
    }
        public async Task<PageResultDto<VideoGameDto>> GetVideoGamesSortedAsync(VideoGameQueryParameters queryParams)
        {
            var games = await _videoGameRepository.GetAllWithDetailsAsQueryableAsync();

            if (queryParams.GenreId.HasValue)
                games = games.Where(g => g.GenreId == queryParams.GenreId);

            if (queryParams.PublisherId.HasValue)
                games = games.Where(g => g.PublisherId == queryParams.PublisherId);

            if (queryParams.MinPrice.HasValue)
                games = games.Where(g => g.Price >= queryParams.MinPrice);

            if (queryParams.MaxPrice.HasValue)
                games = games.Where(g => g.Price <= queryParams.MaxPrice);

            if (!string.IsNullOrEmpty(queryParams.Title))
                games = games.Where(g => g.Title.Contains(queryParams.Title));

            // Sorting
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                games = queryParams.SortBy.ToLower() switch
                {
                    "price" => queryParams.IsDescending ? games.OrderByDescending(g => g.Price) : games.OrderBy(g => g.Price),
                    "releasedate" => queryParams.IsDescending ? games.OrderByDescending(g => g.ReleaseDate) : games.OrderBy(g => g.ReleaseDate),
                    "title" => queryParams.IsDescending ? games.OrderByDescending(g => g.Title) : games.OrderBy(g => g.Title),
                    _ => games.OrderBy(g => g.GameId)
                };
            }
            else
            {
                games = games.OrderBy(g => g.GameId);
            }

            // Pagination
            var totalCount = await games.CountAsync();
            var items = await games
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(g => new VideoGameDto
                {
                    Id = g.GameId,
                    Title = g.Title,
                    Description = g.Description,
                    Price = g.Price,
                    ReleaseDate = g.ReleaseDate,
                    GenreName = g.Genre.Name,
                    PublisherName = g.Publisher.Name
                })
                .ToListAsync();

            return new PageResultDto<VideoGameDto>
            {
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                Items = items
            };
        }
         }
}
