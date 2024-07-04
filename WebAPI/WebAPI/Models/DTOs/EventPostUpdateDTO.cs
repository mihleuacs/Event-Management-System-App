using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs
{
    public class EventPostUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? EventName { get; set; }
        [Required]
        public string? EventDescription { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string? ProductImage { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
