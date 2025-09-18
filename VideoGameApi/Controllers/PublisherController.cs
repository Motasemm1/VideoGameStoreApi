using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoGameApi.Application.DTO.Publisher;
using VideoGameApi.Application.Intefaces;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        { 
            _publisherService = publisherService;
        }

        [HttpGet("GetAllPublishers")]
        public async Task<IActionResult> GetAllPublishers()
        { 
            var publishers = await _publisherService.GetAllPublisher();
            if (publishers == null) 
                return NotFound("NoPublisher found");
            return Ok(publishers);
        }


        [HttpGet("GetPublisherById{id}")]
        public async Task<IActionResult> GetPublisherById(int id)
        { 
            var publisher = await _publisherService.GetPublisherById(id);
            if (publisher == null)
                return NotFound("there is no publisher found");
            return Ok(publisher);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddPublisher")]
        public async Task<IActionResult> AddPublisher(CreatePublisherDto newPublisher)
        {
            if (newPublisher == null)
                return BadRequest("Publisher cant be null");
            var newId = await _publisherService.AddNewPublisher(newPublisher);
            var publisher = await _publisherService.GetPublisherById(newId);
            return CreatedAtAction(nameof(GetPublisherById), new { id = newId }, publisher);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("EditPublisher{id}")]
        public async Task<IActionResult> EditPublisher(int id, UpdatePublisherDto newPublisher)
        {
            if (newPublisher == null)
                return BadRequest("Publisher cant be null");
            var result = await _publisherService.EditPublisher(id, newPublisher);
            if (!result)
                return NotFound("There is No Publisher Found");
            var updatedPublisher = await _publisherService.GetPublisherById(id);
            return Ok(updatedPublisher);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePublisher{id}")]
        public async Task<IActionResult> DeletePublisher(int id) { 
        var result = await _publisherService.DeletePublisherById(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}
