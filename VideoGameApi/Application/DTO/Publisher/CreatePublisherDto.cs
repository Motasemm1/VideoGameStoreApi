using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.Publisher
{
    public class CreatePublisherDto
    {
        [Required]
        public string Name { get; set; }
    }
}
