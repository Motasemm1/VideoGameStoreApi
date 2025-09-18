using VideoGameApi.Application.DTO;
using VideoGameApi.Application.DTO.Publisher;
using VideoGameApi.Application.DTO.Genre;

namespace VideoGameApi.Application.Intefaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDto?>> GetAllPublisher();
        Task<PublisherDto?> GetPublisherById(int id);
        Task<int> AddNewPublisher(CreatePublisherDto newPublisher);
        Task<bool> EditPublisher(int id, UpdatePublisherDto newPublisher);
        Task<Result> DeletePublisherById(int id);
    }
}
