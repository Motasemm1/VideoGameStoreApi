using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoGameApi.Application.DTO.Genre;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Application.DTO.UserDto;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController( IGenreService genreService)
        {
            
            _genreService = genreService;
        }
        [HttpGet("GetAllGenres")]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAllGenres();
            if (!genres.Any())
            {
                return NotFound("No genres found.");
            }
            return Ok(genres);
        }

        [HttpGet("GetGenreById/{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        { 
            var genre = await _genreService.GetGenreById(id);
            if (genre == null)
                return NotFound("There is No Genre Found");
            return Ok(genre);

        }

        [HttpPost("AddNewGenre")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewGenre(CreateGenreDto NewGenre)
        {
            if (NewGenre == null)
                return BadRequest("Genre cant be null");
            var newId = await _genreService.AddNewGenre(NewGenre);
            var genre = await _genreService.GetGenreById(newId);
            return CreatedAtAction(nameof(GetGenreById), new { id = newId }, genre);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("EditGenre{id}")]
        public async Task<IActionResult> EditGenre(int id, UpdateGenreDto newGenre)
        {
           
        if (newGenre == null)
                return BadRequest("Genre cant be null");
            var result = await _genreService.EditGenre(id, newGenre);
            if (!result)
                return NotFound("There is No Genre Found");
            var updatedGenre = await _genreService.GetGenreById(id);
            return Ok(updatedGenre);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteGenreById{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        { 
            var result = await _genreService.DeleteGenreById(id);
            if (!result.Success)
            {
                if (result.Message == "Genre not found.")
                    return NotFound(new { message = result.Message });

                if (result.Message == "Cannot delete genre with associated video games.")
                    return BadRequest(new { message = result.Message });
            }
            return Ok(result.Message );
            
        }
    }
}
