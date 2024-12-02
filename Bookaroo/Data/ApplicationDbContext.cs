using Bookaroo.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;


namespace Bookaroo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets for tables
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring Many-to-Many Relationship
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId }); // Composite Key

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);

            // Seed data for Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, FirstName = "J.K.", LastName = "Rowling", DateOfBirth = new DateTime(1965, 7, 31), Bio = "British author, best known for the Harry Potter series." },
                new Author { AuthorId = 2, FirstName = "George", LastName = "Orwell", DateOfBirth = new DateTime(1903, 6, 25), Bio = "English novelist and essayist, known for his works '1984' and 'Animal Farm'." },
                new Author { AuthorId = 3, FirstName = "J.R.R.", LastName = "Tolkien", DateOfBirth = new DateTime(1892, 1, 3), Bio = "English writer, best known for 'The Hobbit' and 'The Lord of the Rings'." },
                new Author { AuthorId = 4, FirstName = "Isaac", LastName = "Asimov", DateOfBirth = new DateTime(1920, 1, 2), Bio = "Prolific science fiction and popular science author." },
                new Author { AuthorId = 5, FirstName = "Frank", LastName = "Herbert", DateOfBirth = new DateTime(1920, 10, 8), Bio = "American science fiction writer, best known for 'Dune'." },
                new Author { AuthorId = 6, FirstName = "Ray", LastName = "Bradbury", DateOfBirth = new DateTime(1920, 8, 22), Bio = "American author, famous for 'Fahrenheit 451' and short stories." },
                new Author { AuthorId = 7, FirstName = "Aldous", LastName = "Huxley", DateOfBirth = new DateTime(1894, 7, 26), Bio = "British writer and philosopher, known for 'Brave New World'." },
                new Author { AuthorId = 8, FirstName = "William", LastName = "Gibson", DateOfBirth = new DateTime(1948, 3, 17), Bio = "American-Canadian writer, pioneer of the cyberpunk genre." }
            );

            // Seed data for Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Fantasy" },
                new Category { CategoryId = 2, Name = "Science Fiction" },
                new Category { CategoryId = 3, Name = "Dystopian" },
                new Category { CategoryId = 4, Name = "Adventure" },
                new Category { CategoryId = 5, Name = "Cyberpunk" }
            );

            // Seed data for Publishers
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { PublisherId = 1, Name = "Bloomsbury Publishing", Address = "London, UK" },
                new Publisher { PublisherId = 2, Name = "Penguin Books", Address = "New York, USA" },
                new Publisher { PublisherId = 3, Name = "Gollancz", Address = "London, UK" },
                new Publisher { PublisherId = 4, Name = "Tor Books", Address = "New York, USA" }
            );

            // Seed data for Books
            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Harry Potter and the Philosopher's Stone", ISBN = "9780747532699", PublicationDate = new DateTime(1997, 6, 26), Pages = 223, Price = 19.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 2, Title = "1984", ISBN = "9780451524935", PublicationDate = new DateTime(1949, 6, 8), Pages = 328, Price = 9.99m, PublisherId = 2, CategoryId = 3 },
                new Book { BookId = 3, Title = "The Hobbit", ISBN = "9780261102217", PublicationDate = new DateTime(1937, 9, 21), Pages = 310, Price = 14.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 4, Title = "Dune", ISBN = "9780441013593", PublicationDate = new DateTime(1965, 8, 1), Pages = 412, Price = 12.99m, PublisherId = 4, CategoryId = 2 },
                new Book { BookId = 5, Title = "Fahrenheit 451", ISBN = "9780345342966", PublicationDate = new DateTime(1953, 10, 19), Pages = 194, Price = 9.99m, PublisherId = 2, CategoryId = 3 },
                new Book { BookId = 6, Title = "Brave New World", ISBN = "9780060850524", PublicationDate = new DateTime(1932, 8, 30), Pages = 311, Price = 9.99m, PublisherId = 3, CategoryId = 3 },
                new Book { BookId = 7, Title = "Foundation", ISBN = "9780553293357", PublicationDate = new DateTime(1951, 6, 1), Pages = 244, Price = 11.99m, PublisherId = 4, CategoryId = 2 },
                new Book { BookId = 8, Title = "Neuromancer", ISBN = "9780441569595", PublicationDate = new DateTime(1984, 7, 1), Pages = 271, Price = 8.99m, PublisherId = 4, CategoryId = 5 },
                new Book { BookId = 9, Title = "Harry Potter and the Chamber of Secrets", ISBN = "9780747538486", PublicationDate = new DateTime(1998, 7, 2), Pages = 251, Price = 19.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 10, Title = "Harry Potter and the Prisoner of Azkaban", ISBN = "9780747542155", PublicationDate = new DateTime(1999, 7, 8), Pages = 317, Price = 19.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 11, Title = "Harry Potter and the Goblet of Fire", ISBN = "9780747546244", PublicationDate = new DateTime(2000, 7, 8), Pages = 636, Price = 19.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 12, Title = "Harry Potter and the Order of the Phoenix", ISBN = "9780747551002", PublicationDate = new DateTime(2003, 6, 21), Pages = 766, Price = 19.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 13, Title = "Harry Potter and the Half-Blood Prince", ISBN = "9780747581085", PublicationDate = new DateTime(2005, 7, 16), Pages = 607, Price = 19.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 14, Title = "Harry Potter and the Deathly Hallows", ISBN = "9780747591053", PublicationDate = new DateTime(2007, 7, 21), Pages = 759, Price = 19.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 15, Title = "The Silmarillion", ISBN = "9780618126989", PublicationDate = new DateTime(1977, 9, 15), Pages = 365, Price = 14.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 16, Title = "The Fellowship of the Ring", ISBN = "9780261103573", PublicationDate = new DateTime(1954, 7, 29), Pages = 423, Price = 14.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 17, Title = "The Two Towers", ISBN = "9780261102361", PublicationDate = new DateTime(1954, 11, 11), Pages = 352, Price = 14.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 18, Title = "The Return of the King", ISBN = "9780261102378", PublicationDate = new DateTime(1955, 10, 20), Pages = 416, Price = 14.99m, PublisherId = 1, CategoryId = 1 },
                new Book { BookId = 19, Title = "Ender's Game", ISBN = "9780812550702", PublicationDate = new DateTime(1985, 1, 15), Pages = 324, Price = 10.99m, PublisherId = 4, CategoryId = 2 },
                new Book { BookId = 20, Title = "Children of Dune", ISBN = "9780441015610", PublicationDate = new DateTime(1976, 4, 21), Pages = 444, Price = 12.99m, PublisherId = 4, CategoryId = 2 }
            );

            // Seed data for BookAuthor (many-to-many relationship)
            modelBuilder.Entity<BookAuthor>().HasData(
                new BookAuthor { BookId = 1, AuthorId = 1 },  // J.K. Rowling wrote Harry Potter
                new BookAuthor { BookId = 2, AuthorId = 2 },  // George Orwell wrote 1984
                new BookAuthor { BookId = 3, AuthorId = 3 },  // J.R.R. Tolkien wrote The Hobbit
                new BookAuthor { BookId = 4, AuthorId = 5 },  // Frank Herbert wrote Dune
                new BookAuthor { BookId = 5, AuthorId = 6 },  // Ray Bradbury wrote Fahrenheit 451
                new BookAuthor { BookId = 6, AuthorId = 7 },  // Aldous Huxley wrote Brave New World
                new BookAuthor { BookId = 7, AuthorId = 4 },  // Isaac Asimov wrote Foundation
                new BookAuthor { BookId = 8, AuthorId = 8 },  // William Gibson wrote Neuromancer
                new BookAuthor { BookId = 9, AuthorId = 1 },  // J.K. Rowling wrote Harry Potter and the Chamber of Secrets
                new BookAuthor { BookId = 10, AuthorId = 1 }, // J.K. Rowling wrote Harry Potter and the Prisoner of Azkaban
                new BookAuthor { BookId = 11, AuthorId = 1 }, // J.K. Rowling wrote Harry Potter and the Goblet of Fire
                new BookAuthor { BookId = 12, AuthorId = 1 }, // J.K. Rowling wrote Harry Potter and the Order of the Phoenix
                new BookAuthor { BookId = 13, AuthorId = 1 }, // J.K. Rowling wrote Harry Potter and the Half-Blood Prince
                new BookAuthor { BookId = 14, AuthorId = 1 }, // J.K. Rowling wrote Harry Potter and the Deathly Hallows
                new BookAuthor { BookId = 15, AuthorId = 3 }, // J.R.R. Tolkien wrote The Silmarillion
                new BookAuthor { BookId = 16, AuthorId = 3 }, // J.R.R. Tolkien wrote The Fellowship of the Ring
                new BookAuthor { BookId = 17, AuthorId = 3 }, // J.R.R. Tolkien wrote The Two Towers
                new BookAuthor { BookId = 18, AuthorId = 3 }, // J.R.R. Tolkien wrote The Return of the King
                new BookAuthor { BookId = 19, AuthorId = 4 }, // Isaac Asimov wrote Ender's Game
                new BookAuthor { BookId = 20, AuthorId = 5 }  // Frank Herbert wrote Children of Dune
            );
        }

    }
}