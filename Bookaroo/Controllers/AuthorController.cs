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
    public class AuthorController : ControllerBase
    {
        #region Properties
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        #endregion

        #region Constructor
        public AuthorController(ApplicationDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        }
        #endregion

        #region Methods

        #region GetAllAuthors
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var cacheKey = $"Authors_Page{pageNumber}_Size{pageSize}_Search{search ?? "All"}";

            var result = _cacheService.GetOrCreate(cacheKey, () =>
            {
                var query = _context.Authors?.Include(a => a.BookAuthors)?.ThenInclude(ba => ba.Book)?.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim().ToLower();
                    query = query?.Where(a =>
                        a.FirstName.ToLower().Contains(search) ||
                        a.LastName.ToLower().Contains(search));
                }

                var paginatedAuthors = query?.Select(author => new AuthorDto
                {
                    AuthorId = author.AuthorId,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    DateOfBirth = author.DateOfBirth,
                    Bio = author.Bio,
                    BookTitles = author.BookAuthors.Select(ba => ba.Book.Title)
                }).OrderBy(a => a.LastName).ToPaginatedListAsync(pageNumber, pageSize).Result;

                var tempResult = new
                {
                    paginatedAuthors.Items,
                    paginatedAuthors.PageIndex,
                    paginatedAuthors.TotalPages,
                    paginatedAuthors.TotalItems,
                    paginatedAuthors.HasPreviousPage,
                    paginatedAuthors.HasNextPage
                };

                return tempResult;
            }, _cacheOptions);


            return Ok(result);
        }
        #endregion

        #region GetAuthorById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var cacheKey = $"Author_{id}";

            var author = _cacheService.GetOrCreate(cacheKey, () =>
            {
                return _context.Authors?.Include(a => a.BookAuthors)?.ThenInclude(ba => ba.Book)?.FirstOrDefaultAsync(a => a.AuthorId == id).Result;
            }, _cacheOptions);

            if (author == null)
            {
                return NotFound();
            }

            var authorDto = new AuthorDto
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName,
                DateOfBirth = author.DateOfBirth,
                Bio = author.Bio,
                BookTitles = author.BookAuthors.Select(ba => ba.Book.Title).ToList()
            };

            return Ok(authorDto);
        }
        #endregion

        #region CreateAuthor
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = new Author
            {
                FirstName = createAuthorDto.FirstName,
                LastName = createAuthorDto.LastName,
                DateOfBirth = createAuthorDto.DateOfBirth,
                Bio = createAuthorDto.Bio
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // Invalidate cached books list
            _cacheService.InvalidateQueryCaches();
            _cacheService.InvalidateCacheByPrefix("Authors_");

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.AuthorId }, createAuthorDto);
        }
        #endregion

        #region UpdateAuthor
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorDto updateAuthorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            author.FirstName = updateAuthorDto.FirstName;
            author.LastName = updateAuthorDto.LastName;
            author.DateOfBirth = updateAuthorDto.DateOfBirth;
            author.Bio = updateAuthorDto.Bio;

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Invalidate the specific author cache and authors list cache
                _cacheService.Remove($"Author_{id}");
                _cacheService.InvalidateQueryCaches();
                _cacheService.InvalidateCacheByPrefix("Authors_");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        #region DeleteAuthor
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            // Invalidate the specific author cache and authors list cache
            _cacheService.Remove($"Author_{id}");
            _cacheService.InvalidateQueryCaches();
            _cacheService.InvalidateCacheByPrefix("Authors_");

            return NoContent();
        }
        #endregion

        #region AuthorExists
        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(a => a.AuthorId == id);
        }
        #endregion

        #endregion
    }
}
