using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities.DTOs
{
    public class BookDto
    {
        public int BookId { get; set; }

        public required string Title { get; set; }

        public required string ISBN { get; set; }

        public DateTime? PublicationDate { get; set; }

        [Range(1, int.MaxValue)]
        public int? Pages { get; set; }

        public decimal? Price { get; set; }

        public int PublisherId { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<int>? AuthorIds { get; set; }
    }
}
