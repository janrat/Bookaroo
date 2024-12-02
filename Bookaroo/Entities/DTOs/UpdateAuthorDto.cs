using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities.DTOs
{
    public class UpdateAuthorDto
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? Bio { get; set; }
    }
}
