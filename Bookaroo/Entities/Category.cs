using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public required string Name { get; set; }

        // Navigation Properties
        public ICollection<Book>? Books { get; set; }
    }
}
