using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bookaroo.Controllers;
using Bookaroo.Data;
using Bookaroo.Services;
using Bookaroo.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookaroo.Entities;
using Microsoft.Extensions.Caching.Memory;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Http;

namespace Bookaroo.Controllers.Tests
{
    [TestClass]
    public class BookControllerTests
    {
        private ApplicationDbContext _context;
        private Mock<ICacheService> _mockCacheService;
        private Mock<IAuthenticationService> _mockAuthService;
        private BookController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _mockCacheService = new Mock<ICacheService>();
            _mockAuthService = new Mock<IAuthenticationService>();
            _controller = new BookController(_context, _mockCacheService.Object, _mockAuthService.Object);

            var mockHttpContext = new DefaultHttpContext();
            mockHttpContext.Request.Headers["Authorization"] = "Bearer TOZSvv4nOWMZnnyLO3QGQ09CiK6AWjnN";

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };
        }

        [TestMethod]
        public async Task GetAllBooksTest_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);

            //// Seed the in-memory database
            //context.Books.AddRange(new List<Book>
            //{
            //    new Book { BookId = 1, Title = "Test Book", ISBN = "123456", Pages = 200, Price = 19.99M },
            //    new Book { BookId = 2, Title = "Another Book", ISBN = "789012", Pages = 150, Price = 9.99M }
            //});
            //context.SaveChanges();

            List<BookDto> bookDtos = new List<BookDto>();

            foreach (var book in context.Books)
            {
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

                bookDtos.Add(bookDto);
            }
            var mockCacheService = new Mock<ICacheService>();
            var mockAuthService = new Mock<IAuthenticationService>();

            // Mock authorization
            mockAuthService.Setup(a => a.ValidateToken(It.IsAny<string>(), out It.Ref<System.Security.Claims.ClaimsPrincipal>.IsAny)).Returns(true);

            // Mock caching
            mockCacheService.Setup(c => c.GetOrCreate(It.IsAny<string>(), It.IsAny<Func<object>>(), It.IsAny<MemoryCacheEntryOptions>()))
                .Returns(new { Items = bookDtos, PageIndex = 1, TotalPages = 1, TotalItems = 2, HasPreviousPage = false, HasNextPage = false });

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            dynamic data = okResult.Value;
            Assert.AreEqual(2, data.Items.Count); // Ensure there are two books returned
        }

        [TestMethod]
        public async Task GetBookByIdTest_ReturnsNotFound()
        {
            // Arrange
            _mockAuthService.Setup(a => a.ValidateToken(It.IsAny<string>(), out It.Ref<System.Security.Claims.ClaimsPrincipal>.IsAny)).Returns(true);
            _mockCacheService.Setup(c => c.GetOrCreate(It.IsAny<string>(), It.IsAny<Func<object>>(), It.IsAny<MemoryCacheEntryOptions>()))
                .Returns((object)null);

            // Act
            var result = await _controller.GetBookById(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateBookTest_ReturnsCreatedResult()
        {
            // Arrange
            var createBookDto = new CreateBookDto
            {
                Title = "New Book",
                ISBN = "1234567890",
                Pages = 300,
                Price = 29.99M,
                AuthorIds = new List<int> { 1, 2 },
                PublisherId = 1,
                CategoryId = 2
            };
            _mockAuthService.Setup(a => a.ValidateToken(It.IsAny<string>(), out It.Ref<System.Security.Claims.ClaimsPrincipal>.IsAny)).Returns(true);
            _mockAuthService.Setup(a => a.ValidateUserRole(It.IsAny<string>(), "Admin")).Returns(true);

            // Act
            var result = await _controller.CreateBook(createBookDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(createBookDto.Title, ((CreateBookDto)createdResult.Value).Title);
        }

        [TestMethod]
        public async Task UpdateBookTest_ReturnsNoContent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_UpdateBook")
                .Options;

            using var context = new ApplicationDbContext(options);

            // Seed the database
            var book = new Book { BookId = 1, Title = "Old Book", ISBN = "1", Pages = 100, Price = 15.99M };
            context.Books.Add(book);
            context.SaveChanges();

            var updateBookDto = new UpdateBookDto
            {
                Title = "Updated Book",
                ISBN = "0987654321",
                Pages = 250,
                Price = 25.99M,
                AuthorIds = new List<int> { 1 },
                PublisherId = 2,
                CategoryId = 3
            };

            var mockAuthService = new Mock<IAuthenticationService>();
            var mockCacheService = new Mock<ICacheService>();

            // Mock authorization
            mockAuthService.Setup(a => a.ValidateToken(It.IsAny<string>(), out It.Ref<System.Security.Claims.ClaimsPrincipal>.IsAny)).Returns(true);
            mockAuthService.Setup(a => a.ValidateUserRole(It.IsAny<string>(), "Admin")).Returns(true);

            // Act
            var result = await _controller.UpdateBook(1, updateBookDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            // Verify database updates
            var updatedBook = context.Books.FirstOrDefault(b => b.BookId == 1);
            Assert.IsNotNull(updatedBook);
            Assert.AreEqual("Updated Book", updatedBook.Title);
            Assert.AreEqual("0987654321", updatedBook.ISBN);
            Assert.AreEqual(250, updatedBook.Pages);
            Assert.AreEqual(25.99M, updatedBook.Price);
        }


        [TestMethod]
        public async Task DeleteBookTest_ReturnsNoContent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_DeleteBook")
                .Options;

            using var context = new ApplicationDbContext(options);

            // Seed the database
            var book = new Book { BookId = 1, Title = "Old Book", ISBN = "1", Pages = 100, Price = 15.99M };
            context.Books.Add(book);
            context.SaveChanges();

            var mockAuthService = new Mock<IAuthenticationService>();
            var mockCacheService = new Mock<ICacheService>();

            // Mock authorization
            mockAuthService.Setup(a => a.ValidateToken(It.IsAny<string>(), out It.Ref<System.Security.Claims.ClaimsPrincipal>.IsAny)).Returns(true);
            mockAuthService.Setup(a => a.ValidateUserRole(It.IsAny<string>(), "Admin")).Returns(true);

            // Act
            var result = await _controller.DeleteBook(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            // Verify that the book was deleted
            var deletedBook = context.Books.FirstOrDefault(b => b.BookId == 1);
            Assert.IsNull(deletedBook);
        }

    }
}
