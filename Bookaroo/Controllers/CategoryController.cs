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
    public class CategoryController : ControllerBase
    {
        #region Properties
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        #endregion

        #region Constructor
        public CategoryController(ApplicationDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        }
        #endregion

        #region Methods

        #region GetAllCategories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var cacheKey = $"Categories_Page{pageNumber}_Size{pageSize}_Search{search ?? "All"}";

            var result = _cacheService.GetOrCreate(cacheKey, () =>
            {
                var query = _context.Categories.Include(c => c.Books).AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim().ToLower();
                    query = query.Where(c => c.Name.ToLower().Contains(search));
                }

                var paginatedCategories = query.Select(category => new CategoryDto
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    BookTitles = category.Books.Select(b => b.Title).ToList()
                }).OrderBy(c => c.Name).ToPaginatedListAsync(pageNumber, pageSize).Result;

                var tempResult = new
                {
                    paginatedCategories.Items,
                    paginatedCategories.PageIndex,
                    paginatedCategories.TotalPages,
                    paginatedCategories.TotalItems,
                    paginatedCategories.HasPreviousPage,
                    paginatedCategories.HasNextPage
                };

                return tempResult;
            }, _cacheOptions);

            return Ok(result);
        }
        #endregion

        #region GetCategoryById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var cacheKey = $"Category_{id}";

            var category = _cacheService.GetOrCreate(cacheKey, () =>
            {
                return _context.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.CategoryId == id).Result;
            }, _cacheOptions);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                BookTitles = category.Books.Select(b => b.Title).ToList()
            };

            return Ok(categoryDto);
        }
        #endregion

        #region CreateCategory
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Name = createCategoryDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            _cacheService.InvalidateQueryCaches();
            _cacheService.InvalidateCacheByPrefix("Categories_");

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, createCategoryDto);
        }
        #endregion

        #region UpdateCategory
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = updateCategoryDto.Name;

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                _cacheService.Remove($"Category_{id}");
                _cacheService.InvalidateQueryCaches();
                _cacheService.InvalidateCacheByPrefix("Categories_");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        #region DeleteCategory
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            _cacheService.Remove($"Category_{id}");
            _cacheService.InvalidateQueryCaches();
            _cacheService.InvalidateCacheByPrefix("Categories_");

            return NoContent();
        }
        #endregion

        #region CategoryExists
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.CategoryId == id);
        }
        #endregion

        #endregion
    }
}
