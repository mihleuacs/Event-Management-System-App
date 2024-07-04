using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs
{
    public class EventPostDTO
    {
        [Required]
        [MaxLength(30)]
        public string? EventName { get; set; }
        [Required]
        public string? EventDescription { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public string CategoryName { get; set; }
        
        [Required]
        public IFormFile? ImageFile { get; set; }

    }
}
