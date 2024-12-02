using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities.DTOs
{
    public class UpdateCategoryDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
