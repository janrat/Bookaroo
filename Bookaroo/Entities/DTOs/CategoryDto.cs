namespace Bookaroo.Entities.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public required string Name { get; set; }

        public IEnumerable<string>? BookTitles { get; set; }
    }
}
