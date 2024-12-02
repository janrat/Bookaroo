using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookaroo.Data;
using Bookaroo.Extensions;
using Bookaroo.Services;
using Bookaroo.Entities.DTOs;
using System.Threading.Tasks;
using Bookaroo.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Bookaroo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        #region Properties
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        #endregion

        #region Constructor
        public PublisherController(ApplicationDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        }
        #endregion

        #region Methods

        #region GetAllPublishers
        [HttpGet]
        public async Task<IActionResult> GetAllPublishers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var cacheKey = $"Publishers_Page{pageNumber}_Size{pageSize}_Search{search ?? "All"}";

            var result = _cacheService.GetOrCreate(cacheKey, () =>
            {
                var query = _context.Publishers.Include(p => p.Books).AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim().ToLower();
                    query = query.Where(p => p.Name.ToLower().Contains(search));
                }

                var paginatedPublishers = query.Select(publisher => new PublisherDto
                {
                    PublisherId = publisher.PublisherId,
                    Name = publisher.Name,
                    Address = publisher.Address,
                    BookTitles = publisher.Books.Select(b => b.Title).ToList()
                }).OrderBy(p => p.Name).ToPaginatedListAsync(pageNumber, pageSize).Result;

                var tempResult = new
                {
                    paginatedPublishers.Items,
                    paginatedPublishers.PageIndex,
                    paginatedPublishers.TotalPages,
                    paginatedPublishers.TotalItems,
                    paginatedPublishers.HasPreviousPage,
                    paginatedPublishers.HasNextPage
                };

                return tempResult;
            }, _cacheOptions);

            return Ok(result);
        }
        #endregion

        #region GetPublisherById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            var cacheKey = $"Publisher_{id}";

            var publisher = _cacheService.GetOrCreate(cacheKey, () =>
            {
                return _context.Publishers.Include(p => p.Books).FirstOrDefaultAsync(p => p.PublisherId == id).Result;
            }, _cacheOptions);
            
            if (publisher == null)
            {
                return NotFound();
            }

            var publisherDto = new PublisherDto
            {
                PublisherId = publisher.PublisherId,
                Name = publisher.Name,
                Address = publisher.Address,
                BookTitles = publisher.Books.Select(b => b.Title).ToList()
            };

            return Ok(publisherDto);
        }
        #endregion

        #region CreatePublisher
        [HttpPost]
        public async Task<IActionResult> CreatePublisher([FromBody] CreatePublisherDto createPublisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = new Publisher
            {
                Name = createPublisherDto.Name,
                Address = createPublisherDto.Address
            };

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            _cacheService.InvalidateQueryCaches();
            _cacheService.InvalidateCacheByPrefix("Publishers_");

            return CreatedAtAction(nameof(GetPublisherById), new { id = publisher.PublisherId }, createPublisherDto);
        }
        #endregion

        #region UpdatePublisher
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] UpdatePublisherDto updatePublisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            publisher.Name = updatePublisherDto.Name;
            publisher.Address = updatePublisherDto.Address;

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                _cacheService.Remove($"Publisher_{id}");
                _cacheService.InvalidateQueryCaches();
                _cacheService.InvalidateCacheByPrefix("Publishers_");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion

        #region DeletePublisher
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            _cacheService.Remove($"Publisher_{id}");
            _cacheService.InvalidateQueryCaches();
            _cacheService.InvalidateCacheByPrefix("Publishers_");

            return NoContent();
        }
        #endregion

        #region PublisherExists
        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(p => p.PublisherId == id);
        }
        #endregion

        #endregion
    }
}
