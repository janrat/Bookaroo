﻿// <auto-generated />
using System;
using Bookaroo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bookaroo.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241201122108_SeedDataFix")]
    partial class SeedDataFix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bookaroo.Entities.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            AuthorId = 1,
                            Bio = "British author, best known for the Harry Potter series.",
                            DateOfBirth = new DateTime(1965, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "J.K.",
                            LastName = "Rowling"
                        },
                        new
                        {
                            AuthorId = 2,
                            Bio = "English novelist and essayist, known for his works '1984' and 'Animal Farm'.",
                            DateOfBirth = new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "George",
                            LastName = "Orwell"
                        },
                        new
                        {
                            AuthorId = 3,
                            Bio = "English writer, best known for 'The Hobbit' and 'The Lord of the Rings'.",
                            DateOfBirth = new DateTime(1892, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "J.R.R.",
                            LastName = "Tolkien"
                        },
                        new
                        {
                            AuthorId = 4,
                            Bio = "Prolific science fiction and popular science author.",
                            DateOfBirth = new DateTime(1920, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Isaac",
                            LastName = "Asimov"
                        },
                        new
                        {
                            AuthorId = 5,
                            Bio = "American science fiction writer, best known for 'Dune'.",
                            DateOfBirth = new DateTime(1920, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Frank",
                            LastName = "Herbert"
                        },
                        new
                        {
                            AuthorId = 6,
                            Bio = "American author, famous for 'Fahrenheit 451' and short stories.",
                            DateOfBirth = new DateTime(1920, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Ray",
                            LastName = "Bradbury"
                        },
                        new
                        {
                            AuthorId = 7,
                            Bio = "British writer and philosopher, known for 'Brave New World'.",
                            DateOfBirth = new DateTime(1894, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Aldous",
                            LastName = "Huxley"
                        },
                        new
                        {
                            AuthorId = 8,
                            Bio = "American-Canadian writer, pioneer of the cyberpunk genre.",
                            DateOfBirth = new DateTime(1948, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "William",
                            LastName = "Gibson"
                        });
                });

            modelBuilder.Entity("Bookaroo.Entities.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Pages")
                        .HasColumnType("int");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            CategoryId = 1,
                            ISBN = "9780747532699",
                            Pages = 223,
                            Price = 19.99m,
                            PublicationDate = new DateTime(1997, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "Harry Potter and the Philosopher's Stone"
                        },
                        new
                        {
                            BookId = 2,
                            CategoryId = 3,
                            ISBN = "9780451524935",
                            Pages = 328,
                            Price = 9.99m,
                            PublicationDate = new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 2,
                            Title = "1984"
                        },
                        new
                        {
                            BookId = 3,
                            CategoryId = 1,
                            ISBN = "9780261102217",
                            Pages = 310,
                            Price = 14.99m,
                            PublicationDate = new DateTime(1937, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "The Hobbit"
                        },
                        new
                        {
                            BookId = 4,
                            CategoryId = 2,
                            ISBN = "9780441013593",
                            Pages = 412,
                            Price = 12.99m,
                            PublicationDate = new DateTime(1965, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 4,
                            Title = "Dune"
                        },
                        new
                        {
                            BookId = 5,
                            CategoryId = 3,
                            ISBN = "9780345342966",
                            Pages = 194,
                            Price = 9.99m,
                            PublicationDate = new DateTime(1953, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 2,
                            Title = "Fahrenheit 451"
                        },
                        new
                        {
                            BookId = 6,
                            CategoryId = 3,
                            ISBN = "9780060850524",
                            Pages = 311,
                            Price = 9.99m,
                            PublicationDate = new DateTime(1932, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 3,
                            Title = "Brave New World"
                        },
                        new
                        {
                            BookId = 7,
                            CategoryId = 2,
                            ISBN = "9780553293357",
                            Pages = 244,
                            Price = 11.99m,
                            PublicationDate = new DateTime(1951, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 4,
                            Title = "Foundation"
                        },
                        new
                        {
                            BookId = 8,
                            CategoryId = 5,
                            ISBN = "9780441569595",
                            Pages = 271,
                            Price = 8.99m,
                            PublicationDate = new DateTime(1984, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 4,
                            Title = "Neuromancer"
                        },
                        new
                        {
                            BookId = 9,
                            CategoryId = 1,
                            ISBN = "9780747538486",
                            Pages = 251,
                            Price = 19.99m,
                            PublicationDate = new DateTime(1998, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "Harry Potter and the Chamber of Secrets"
                        },
                        new
                        {
                            BookId = 10,
                            CategoryId = 1,
                            ISBN = "9780747542155",
                            Pages = 317,
                            Price = 19.99m,
                            PublicationDate = new DateTime(1999, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "Harry Potter and the Prisoner of Azkaban"
                        },
                        new
                        {
                            BookId = 11,
                            CategoryId = 1,
                            ISBN = "9780747546244",
                            Pages = 636,
                            Price = 19.99m,
                            PublicationDate = new DateTime(2000, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "Harry Potter and the Goblet of Fire"
                        },
                        new
                        {
                            BookId = 12,
                            CategoryId = 1,
                            ISBN = "9780747551002",
                            Pages = 766,
                            Price = 19.99m,
                            PublicationDate = new DateTime(2003, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "Harry Potter and the Order of the Phoenix"
                        },
                        new
                        {
                            BookId = 13,
                            CategoryId = 1,
                            ISBN = "9780747581085",
                            Pages = 607,
                            Price = 19.99m,
                            PublicationDate = new DateTime(2005, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "Harry Potter and the Half-Blood Prince"
                        },
                        new
                        {
                            BookId = 14,
                            CategoryId = 1,
                            ISBN = "9780747591053",
                            Pages = 759,
                            Price = 19.99m,
                            PublicationDate = new DateTime(2007, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "Harry Potter and the Deathly Hallows"
                        },
                        new
                        {
                            BookId = 15,
                            CategoryId = 1,
                            ISBN = "9780618126989",
                            Pages = 365,
                            Price = 14.99m,
                            PublicationDate = new DateTime(1977, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "The Silmarillion"
                        },
                        new
                        {
                            BookId = 16,
                            CategoryId = 1,
                            ISBN = "9780261103573",
                            Pages = 423,
                            Price = 14.99m,
                            PublicationDate = new DateTime(1954, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "The Fellowship of the Ring"
                        },
                        new
                        {
                            BookId = 17,
                            CategoryId = 1,
                            ISBN = "9780261102361",
                            Pages = 352,
                            Price = 14.99m,
                            PublicationDate = new DateTime(1954, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "The Two Towers"
                        },
                        new
                        {
                            BookId = 18,
                            CategoryId = 1,
                            ISBN = "9780261102378",
                            Pages = 416,
                            Price = 14.99m,
                            PublicationDate = new DateTime(1955, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 1,
                            Title = "The Return of the King"
                        },
                        new
                        {
                            BookId = 19,
                            CategoryId = 2,
                            ISBN = "9780812550702",
                            Pages = 324,
                            Price = 10.99m,
                            PublicationDate = new DateTime(1985, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 4,
                            Title = "Ender's Game"
                        },
                        new
                        {
                            BookId = 20,
                            CategoryId = 2,
                            ISBN = "9780441015610",
                            Pages = 444,
                            Price = 12.99m,
                            PublicationDate = new DateTime(1976, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublisherId = 4,
                            Title = "Children of Dune"
                        });
                });

            modelBuilder.Entity("Bookaroo.Entities.BookAuthor", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.HasKey("BookId", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.ToTable("BookAuthors");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            AuthorId = 1
                        },
                        new
                        {
                            BookId = 2,
                            AuthorId = 2
                        },
                        new
                        {
                            BookId = 3,
                            AuthorId = 3
                        },
                        new
                        {
                            BookId = 4,
                            AuthorId = 5
                        },
                        new
                        {
                            BookId = 5,
                            AuthorId = 6
                        },
                        new
                        {
                            BookId = 6,
                            AuthorId = 7
                        },
                        new
                        {
                            BookId = 7,
                            AuthorId = 4
                        },
                        new
                        {
                            BookId = 8,
                            AuthorId = 8
                        },
                        new
                        {
                            BookId = 9,
                            AuthorId = 1
                        },
                        new
                        {
                            BookId = 10,
                            AuthorId = 1
                        },
                        new
                        {
                            BookId = 11,
                            AuthorId = 1
                        },
                        new
                        {
                            BookId = 12,
                            AuthorId = 1
                        },
                        new
                        {
                            BookId = 13,
                            AuthorId = 1
                        },
                        new
                        {
                            BookId = 14,
                            AuthorId = 1
                        },
                        new
                        {
                            BookId = 15,
                            AuthorId = 3
                        },
                        new
                        {
                            BookId = 16,
                            AuthorId = 3
                        },
                        new
                        {
                            BookId = 17,
                            AuthorId = 3
                        },
                        new
                        {
                            BookId = 18,
                            AuthorId = 3
                        },
                        new
                        {
                            BookId = 19,
                            AuthorId = 4
                        },
                        new
                        {
                            BookId = 20,
                            AuthorId = 5
                        });
                });

            modelBuilder.Entity("Bookaroo.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            Name = "Fantasy"
                        },
                        new
                        {
                            CategoryId = 2,
                            Name = "Science Fiction"
                        },
                        new
                        {
                            CategoryId = 3,
                            Name = "Dystopian"
                        },
                        new
                        {
                            CategoryId = 4,
                            Name = "Adventure"
                        },
                        new
                        {
                            CategoryId = 5,
                            Name = "Cyberpunk"
                        });
                });

            modelBuilder.Entity("Bookaroo.Entities.Publisher", b =>
                {
                    b.Property<int>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PublisherId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PublisherId");

                    b.ToTable("Publishers");

                    b.HasData(
                        new
                        {
                            PublisherId = 1,
                            Address = "London, UK",
                            Name = "Bloomsbury Publishing"
                        },
                        new
                        {
                            PublisherId = 2,
                            Address = "New York, USA",
                            Name = "Penguin Books"
                        },
                        new
                        {
                            PublisherId = 3,
                            Address = "London, UK",
                            Name = "Gollancz"
                        },
                        new
                        {
                            PublisherId = 4,
                            Address = "New York, USA",
                            Name = "Tor Books"
                        });
                });

            modelBuilder.Entity("Bookaroo.Entities.Book", b =>
                {
                    b.HasOne("Bookaroo.Entities.Category", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bookaroo.Entities.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("Bookaroo.Entities.BookAuthor", b =>
                {
                    b.HasOne("Bookaroo.Entities.Author", "Author")
                        .WithMany("BookAuthors")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bookaroo.Entities.Book", "Book")
                        .WithMany("BookAuthors")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Bookaroo.Entities.Author", b =>
                {
                    b.Navigation("BookAuthors");
                });

            modelBuilder.Entity("Bookaroo.Entities.Book", b =>
                {
                    b.Navigation("BookAuthors");
                });

            modelBuilder.Entity("Bookaroo.Entities.Category", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Bookaroo.Entities.Publisher", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
