using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Models;
using WebAPI.Models.DTOs;
using WebAPI.Services.Interface;
using WebAPI.Services.Repos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventPostController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IEventPostRepository _eventPost;
        private readonly ILogger<EventPostController> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public EventPostController(IFileService fileService, IEventPostRepository eventPost, ILogger<EventPostController> logger , ICategoryRepository categoryRepository)
        {
            _fileService = fileService;
            _eventPost = eventPost;
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventPost()
        {
            var epost = await _eventPost.GetEventPostsAsync();
            return Ok(epost);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventPost(int id)
        {
            var epost = await _eventPost.FindEventPostByIdAsync(id);
            if (epost == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Event with id: {id} not found");
            }

            return Ok(epost);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventPost([FromForm] EventPostDTO eventToAdd)
        {
            try
            {
                if (eventToAdd.ImageFile?.Length > 1 * 1024 * 1024)
                {
                    return BadRequest(new { message = "File size should not exceed 1 MB" });
                }

                string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png" };
                string createdImageName = await _fileService.SaveFileAsync(eventToAdd.ImageFile, allowedFileExtensions);

                var category = await _categoryRepository.FindCategoryByNameAsync(eventToAdd.CategoryName);
                if (category == null)
                {
                    return NotFound(new { message = $"Category with name: {eventToAdd.CategoryName} not found" });
                }

                var eventPost = new EventPost
                {
                    EventName = eventToAdd.EventName,
                    EventDescription = eventToAdd.EventDescription,
                    CreatedDate = eventToAdd.CreatedDate ?? DateTime.UtcNow,
                    EndDate = eventToAdd.EndDate ?? DateTime.UtcNow.AddDays(1),
                    CategoryId = category.Id,
                    ProductImage = createdImageName
                };

                var createdEventPost = await _eventPost.AddEventPostAsync(eventPost);
                return CreatedAtAction(nameof(GetEventPost), new { id = createdEventPost.Id }, createdEventPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event post");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }



    
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventPost(int id, [FromForm] EventPostUpdateDTO eventToUpdate)
        {
            try
            {
                if (id != eventToUpdate.Id)
                {
                    return BadRequest(new { message = "ID in URL and body do not match" });
                }

                var existingEventPost = await _eventPost.FindEventPostByIdAsync(id);
                if (existingEventPost == null)
                {
                    return NotFound(new { message = $"Event with id: {id} not found" });
                }

                string oldImage = existingEventPost.ProductImage;
                if (eventToUpdate.ImageFile != null)
                {
                    if (eventToUpdate.ImageFile?.Length > 1 * 1024 * 1024)
                    {
                        return BadRequest(new { message = "File size should not exceed 1 MB" });
                    }
                    string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png" };
                    string newImageName = await _fileService.SaveFileAsync(eventToUpdate.ImageFile, allowedFileExtensions);
                    eventToUpdate.ProductImage = newImageName;
                }

                existingEventPost.EventName = eventToUpdate.EventName;
                existingEventPost.EventDescription = eventToUpdate.EventDescription;
                existingEventPost.CreatedDate = eventToUpdate.CreatedDate;
                existingEventPost.EndDate = eventToUpdate.EndDate;
                existingEventPost.ProductImage = eventToUpdate.ProductImage;

                var updatedEventPost = await _eventPost.UpdateEventPostAsync(existingEventPost);

                if (eventToUpdate.ImageFile != null)
                {
                    _fileService.DeleteFile(oldImage);
                }

                return Ok(updatedEventPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event post");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventPost(int id)
        {
            try
            {
                var existingEventPost = await _eventPost.FindEventPostByIdAsync(id);
                if (existingEventPost == null)
                {
                    return NotFound(new { message = $"Event with id: {id} not found" });
                }

                await _eventPost.DeleteEventPostAsync(existingEventPost);
                _fileService.DeleteFile(existingEventPost.ProductImage);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event post");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

    }
}

