using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string? Bio { get; set; }

        // Navigation Properties
        public ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}
