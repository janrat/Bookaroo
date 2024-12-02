namespace Bookaroo.Entities.DTOs
{
    public class PublisherDto
    {
        public int PublisherId { get; set; }

        public required string Name { get; set; }

        public required string Address { get; set; }

        public IEnumerable<string>? BookTitles { get; set; }
    }
}
