using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.Genre
{
    public class CreateGenreDto
    {
        [Required]
        public string Name { get; set; }
    }
}
