using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class EventPost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? EventName { get; set; }
        [Required]
        public string? EventDescription { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set;}
        [Required]
        public string? ProductImage { get; set; }
         

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
