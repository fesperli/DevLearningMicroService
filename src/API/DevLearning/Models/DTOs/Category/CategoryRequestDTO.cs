using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs.Category
{
    public class CategoryRequestDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Featured { get; set; }
    }
}
