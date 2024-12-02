namespace Bookaroo.Entities.DTOs
{
    public class AuthorDto
    {
        public int AuthorId { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Bio { get; set; }

        public IEnumerable<string>? BookTitles { get; set; }
    }
}
