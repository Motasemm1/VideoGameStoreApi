using Microsoft.EntityFrameworkCore;
using VideoGameApi.Application.DTO;
using VideoGameApi.Application.DTO.Publisher;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Application.DTO.GameDto;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;
using VideoGameApi.Infrastructure.Repositories;

namespace VideoGameApi.Application.Service
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }
        public async Task<int> AddNewPublisher(CreatePublisherDto newPublisher)
        {
            var publisher = new Publisher
            {
                Name = newPublisher.Name
            };
            await _publisherRepository.AddAsync(publisher);
            return publisher.PublisherId;
        }

        public async Task<Result> DeletePublisherById(int id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);
            if (publisher == null)
                return new Result { Success = false, Message = "Publisher not found" };
            if (publisher.VideoGames.Any())
                return new Result { Success = false, Message = "Cannot delete Publisher with associated video games" };
            
           await _publisherRepository.DeleteAsync(publisher);
            return new Result { Success = true, Message = "Publisher deleted successfully" };
        }

        public async Task<bool> EditPublisher(int id, UpdatePublisherDto newPublisher)
        {
            var oldPublisher =await  _publisherRepository.GetByIdAsync(id);
            if (oldPublisher == null)
                return false;
            oldPublisher.Name = newPublisher.Name;
           await _publisherRepository.UpdateAsync(oldPublisher);
            return true;
        }

        public async Task<IEnumerable<PublisherDto?>> GetAllPublisher()
        {
            var allpublishers =  await _publisherRepository.GetAllAsync();
            if (allpublishers == null)
                return null;
            var publishers = allpublishers
                .Select(p => new PublisherDto
                {
                    PublisherId = p.PublisherId,
                    Name = p.Name,
                    VideoGames = p.VideoGames.Select(v => new VideoGameDto
                    {
                        Id = v.GameId,
                        Description = v.Description,
                        GenreName = v.Genre.Name,
                        Price = v.Price,
                        PublisherName = v.Publisher.Name,
                        ReleaseDate = v.ReleaseDate,
                        Title = v.Title
                    }).ToList()
                });
            return publishers;
        }

        public async Task<PublisherDto?> GetPublisherById(int id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);
            if (publisher == null)
                return null;
            var publisherDto = new PublisherDto
            {
                PublisherId = publisher.PublisherId,
                Name = publisher.Name,
                VideoGames = publisher.VideoGames.Select(v => new VideoGameDto
                {
                    Id = v.GameId,
                    Description = v.Description,
                    GenreName = v.Genre.Name,
                    Price = v.Price,
                    PublisherName = v.Publisher.Name,
                    ReleaseDate = v.ReleaseDate,
                    Title = v.Title
                }).ToList()
            };
            return publisherDto;

        }
    }
}
