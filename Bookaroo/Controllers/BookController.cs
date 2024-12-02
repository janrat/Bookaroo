using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookaroo.Data;
using Bookaroo.Services;
using Bookaroo.Entities.DTOs;
using Bookaroo.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Bookaroo.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Bookaroo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        #region Properties
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        private readonly IAuthenticationService _authService;
        #endregion

        #region Constructor
        public BookController(ApplicationDbContext context, ICacheService cacheService, IAuthenticationService authService)
        {
            _context = context;
            _cacheService = cacheService;
            _authService = authService;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));
        }
        #endregion

        #region Methods

        #region GetAllBooks
        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            // Validate token directly
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if (!_authService.ValidateToken(token, out var principal))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var cacheKey = $"Books_Page{pageNumber}_Size{pageSize}_Search{search ?? "All"}";

            var result = _cacheService.GetOrCreate(cacheKey, () =>
            {
                var query = _context.Books?.Include(b => b.Publisher)?.Include(b => b.Category)?.Include(b => b.BookAuthors).ThenInclude(ba => ba.Author).AsQueryable();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim().ToLower();
                    query = query?.Where(b =>
                        b.Title.ToLower().Contains(search));
                }

                var paginatedBooks = query?.Select(book => new BookDto
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    PublicationDate = book.PublicationDate,
                    Pages = book.Pages,
                    Price = book.Price,
                    PublisherId = book.PublisherId,
                    CategoryId = book.CategoryId,
                    AuthorIds = book.BookAuthors != null ? book.BookAuthors.Select(ba => ba.AuthorId).ToList() : new List<int>()
                }).OrderBy(b => b.Title).ToPaginatedListAsync(pageNumber, pageSize).Result;

                var tempResult = new
                {
                    paginatedBooks.Items,
                    paginatedBooks.PageIndex,
                    paginatedBooks.TotalPages,
                    paginatedBooks.TotalItems,
                    paginatedBooks.HasPreviousPage,
                    paginatedBooks.HasNextPage
                };

                return tempResult;
            }, _cacheOptions);

            return Ok(result);
        }
        #endregion

        #region GetBookById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            // Validate token directly
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if (!_authService.ValidateToken(token, out var principal))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var cacheKey = $"Book_{id}";

            var book = _cacheService.GetOrCreate(cacheKey, () =>
            {
                return _context.Books?.Include(b => b.Publisher)?.Include(b => b.Category)?.Include(b => b.BookAuthors)?.ThenInclude(ba => ba.Author)?.FirstOrDefaultAsync(b => b.BookId == id).Result;
            }, _cacheOptions);

            if (book == null)
            {
                return NotFound();
            }

            var bookDto = new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                ISBN = book.ISBN,
                PublicationDate = book.PublicationDate,
                Pages = book.Pages,
                Price = book.Price,
                PublisherId = book.PublisherId,
                CategoryId = book.CategoryId,
                AuthorIds = book.BookAuthors != null ? book.BookAuthors.Select(ba => ba.AuthorId).ToList() : new List<int>()
            };

            return Ok(bookDto);
        }
        #endregion

        #region CreateBook
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
        {
            // Validate token directly and check if the user is an Admin
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if (!_authService.ValidateToken(token, out var principal) || !_authService.ValidateUserRole(token, "Admin"))
            {
                return Unauthorized(new { message = "Admin role is required" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Title = createBookDto.Title,
                ISBN = createBookDto.ISBN,
                PublicationDate = createBookDto.PublicationDate,
                Pages = createBookDto.Pages,
                Price = createBookDto.Price,
                PublisherId = createBookDto.PublisherId,
                CategoryId = createBookDto.CategoryId,
                BookAuthors = createBookDto.AuthorIds.Select(authorId => new BookAuthor { AuthorId = authorId }).ToList()
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            _cacheService.InvalidateQueryCaches();
            _cacheService.InvalidateCacheByPrefix("Books_");

            return CreatedAtAction(nameof(GetBookById), new { id = book.BookId }, createBookDto);
        }
        #endregion

        #region UpdateBook
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
        {
            // Validate token directly and check if the user is an Admin
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if (!_authService.ValidateToken(token, out var principal) || !_authService.ValidateUserRole(token, "Admin"))
            {
                return Unauthorized(new { message = "Admin role is required" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            // Update fields
            book.Title = updateBookDto.Title;
            book.ISBN = updateBookDto.ISBN;
            book.PublicationDate = updateBookDto.PublicationDate;
            book.Pages = updateBookDto.Pages;
            book.Price = updateBookDto.Price;
            book.PublisherId = updateBookDto.PublisherId;
            book.CategoryId = updateBookDto.CategoryId;

            // Remove existing authors
            var existingAuthors = _context.BookAuthors.Where(ba => ba.BookId == id).ToList();
            _context.BookAuthors.RemoveRange(existingAuthors);

            // Add new authors
            var newAuthors = updateBookDto.AuthorIds.Select(authorId => new BookAuthor
            {
                AuthorId = authorId,
                BookId = id
            }).ToList();
            _context.BookAuthors.AddRange(newAuthors);

            try
            {
                await _context.SaveChangesAsync();

                // Invalidate specific book cache and books list cache
                _cacheService.Remove($"Book_{id}");
                _cacheService.InvalidateQueryCaches();
                _cacheService.InvalidateCacheByPrefix("Books_");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        #region DeleteBook
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            // Validate token directly and check if the user is an Admin
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if (!_authService.ValidateToken(token, out var principal) || !_authService.ValidateUserRole(token, "Admin"))
            {
                return Unauthorized(new { message = "Admin role is required" });
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            // Invalidate specific book cache and books list cache
            _cacheService.Remove($"Book_{id}");
            _cacheService.InvalidateQueryCaches();
            _cacheService.InvalidateCacheByPrefix("Books_");

            return NoContent();
        }
        #endregion

        #region BookExists
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
        #endregion

        #endregion
    }
}
