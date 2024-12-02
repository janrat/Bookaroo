using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities.DTOs
{
    public class UpdatePublisherDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Address { get; set; }
    }
}
