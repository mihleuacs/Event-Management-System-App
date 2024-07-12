using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.DTOs;
using WebAPI.Services.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.FindCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = $"Category with id: {id} not found" });
            }
            return Ok(category);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDto)
        {
            try
            {
                var category = new Category
                {
                    CategoryName = categoryDto.CategoryName
                };

                var createdCategory = await _categoryRepository.AddCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryDto)
        {
            try
            {
                if (id != categoryDto.Id)
                {
                    return BadRequest(new { message = "ID in URL and body do not match" });
                }

                var existingCategory = await _categoryRepository.FindCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(new { message = $"Category with id: {id} not found" });
                }

                existingCategory.CategoryName = categoryDto.CategoryName;

                var updatedCategory = await _categoryRepository.UpdateCategoryAsync(existingCategory);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var existingCategory = await _categoryRepository.FindCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(new { message = $"Category with id: {id} not found" });
                }

                await _categoryRepository.DeleteCategoryAsync(existingCategory);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
