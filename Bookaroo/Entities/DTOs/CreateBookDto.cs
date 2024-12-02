using System.ComponentModel.DataAnnotations;

namespace Bookaroo.Entities.DTOs
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string ISBN { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublicationDate { get; set; }

        [Range(1, int.MaxValue)]
        public int? Pages { get; set; }

        public decimal? Price { get; set; }

        [Required]
        public int PublisherId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public List<int> AuthorIds { get; set; } = new List<int>();
    }

}
