using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string ISBN { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublicationDate { get; set; }

        public int? Pages { get; set; }

        public decimal? Price { get; set; }

        // Navigation Properties
        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<BookAuthor>? BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
