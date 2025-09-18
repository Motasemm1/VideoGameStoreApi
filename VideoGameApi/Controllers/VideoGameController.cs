using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoGameApi.Application.DTO.GameDto;
using VideoGameApi.Application.Intefaces;
using VideoGameApi.Domain;
using VideoGameApi.Infrastructure.Data;

namespace VideoGameApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly IVideoGameService _videoGameService;
        private readonly VideoGameDbContext _context;

        public VideoGameController(VideoGameDbContext context,IVideoGameService videoGameService)
        {
            _context = context;
            _videoGameService = videoGameService;
        }

        [HttpGet("GetAllGames")]
        public async Task<ActionResult<List<VideoGame>>> GetAllVideoGames()
        {
          var games = await _videoGameService.GetAllAsync();
            if (games == null )
                return NotFound("No games available.");
            return Ok(games);
        }
        [HttpGet("GetGamesSorted")]
        public async Task<IActionResult> GetAll([FromQuery] VideoGameQueryParameters queryParameters)
        {
            var games = await _videoGameService.GetVideoGamesSortedAsync(queryParameters);
            return Ok(games);
        }

        [HttpGet("GetGameById/{id}")]
        public async Task<ActionResult<VideoGame>> GetVideoGameById(int id)
        {
            var videoGame = await _videoGameService.GetByIdAsync(id);

            if (videoGame == null)
                return NotFound();
            return Ok(videoGame);
        }



        [HttpPost("AddNewGame")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task <ActionResult<VideoGame>> AddNewGame([FromForm] CreateGameDto videoGameDto)
        {
            if (videoGameDto == null)
                return BadRequest("Video game cannot be null.");

            var newId = await _videoGameService.AddVideoGameAsync(videoGameDto);
            var game = await _videoGameService.GetByIdAsync(newId);
            return CreatedAtAction(nameof(GetVideoGameById), new { id = newId }, game);
          }

        [HttpPut("UpdateVideoGame/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task< ActionResult<VideoGame>> UpdateVideoGame(int id, [FromForm] UpdateVideoGameDto newGameDto)
        {
            var result = await _videoGameService.UpdateGameAsync(id, newGameDto);
            if (!result) return NotFound();
            return NoContent();
        }
        
        [HttpDelete("DeleteVideoGame/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task< ActionResult> DeleteVideoGame(int id)
        {
            var result = await _videoGameService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        
        [HttpGet("SearchByName")]
        public async Task<ActionResult> SearchByName (string GameName)
        {
            var games = await _videoGameService.SearchByName(GameName);
            if (games == null || !games.Any())
                return NotFound("No games found matching the search criteria.");
            return Ok(games);
        }

       
    }
}
