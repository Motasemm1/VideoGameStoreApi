using System.ComponentModel.DataAnnotations;

namespace VideoGameApi.Application.DTO.Genre
{
    public class UpdateGenreDto
    {
        [Required]
        public string Name { get; set; }
    }
}
