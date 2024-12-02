using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
