using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.Publisher
{
    public class UpdatePublisherDto
    {
        [Required]
        public string Name { get; set; }
    }
}
