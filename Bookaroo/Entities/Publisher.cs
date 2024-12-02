using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Address { get; set; }

        // Navigation Properties
        public ICollection<Book>? Books { get; set; }
    }
}
