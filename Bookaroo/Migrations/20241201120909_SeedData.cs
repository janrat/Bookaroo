using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookaroo.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Bio", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 4, "Prolific science fiction and popular science author.", new DateTime(1920, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Isaac", "Asimov" },
                    { 5, "American science fiction writer, best known for 'Dune'.", new DateTime(1920, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Frank", "Herbert" },
                    { 6, "American author, famous for 'Fahrenheit 451' and short stories.", new DateTime(1920, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ray", "Bradbury" },
                    { 7, "British writer and philosopher, known for 'Brave New World'.", new DateTime(1894, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aldous", "Huxley" },
                    { 8, "American-Canadian writer, pioneer of the cyberpunk genre.", new DateTime(1948, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "William", "Gibson" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "CategoryId", "ISBN", "Pages", "Price", "PublicationDate", "PublisherId", "Title" },
                values: new object[,]
                {
                    { 5, 3, "9780345342966", 194, 9.99m, new DateTime(1953, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Fahrenheit 451" },
                    { 9, 1, "9780747538486", 251, 19.99m, new DateTime(1998, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Harry Potter and the Chamber of Secrets" },
                    { 10, 1, "9780747542155", 317, 19.99m, new DateTime(1999, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Harry Potter and the Prisoner of Azkaban" },
                    { 11, 1, "9780747546244", 636, 19.99m, new DateTime(2000, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Harry Potter and the Goblet of Fire" },
                    { 12, 1, "9780747551002", 766, 19.99m, new DateTime(2003, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Harry Potter and the Order of the Phoenix" },
                    { 13, 1, "9780747581085", 607, 19.99m, new DateTime(2005, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Harry Potter and the Half-Blood Prince" },
                    { 14, 1, "9780747591053", 759, 19.99m, new DateTime(2007, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Harry Potter and the Deathly Hallows" },
                    { 15, 1, "9780618126989", 365, 14.99m, new DateTime(1977, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "The Silmarillion" },
                    { 16, 1, "9780261103573", 423, 14.99m, new DateTime(1954, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "The Fellowship of the Ring" },
                    { 17, 1, "9780261102361", 352, 14.99m, new DateTime(1954, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "The Two Towers" },
                    { 18, 1, "9780261102378", 416, 14.99m, new DateTime(1955, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "The Return of the King" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 4, "Adventure" },
                    { 5, "Cyberpunk" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "PublisherId", "Address", "Name" },
                values: new object[,]
                {
                    { 3, "London, UK", "Gollancz" },
                    { 4, "New York, USA", "Tor Books" }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[] { 6, 5 });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "CategoryId", "ISBN", "Pages", "Price", "PublicationDate", "PublisherId", "Title" },
                values: new object[,]
                {
                    { 4, 2, "9780441013593", 412, 12.99m, new DateTime(1965, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Dune" },
                    { 6, 3, "9780060850524", 311, 9.99m, new DateTime(1932, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Brave New World" },
                    { 7, 2, "9780553293357", 244, 11.99m, new DateTime(1951, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Foundation" },
                    { 8, 5, "9780441569595", 271, 8.99m, new DateTime(1984, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Neuromancer" },
                    { 19, 2, "9780812550702", 324, 10.99m, new DateTime(1985, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Ender's Game" },
                    { 20, 2, "9780441015610", 444, 12.99m, new DateTime(1976, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Children of Dune" }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 5, 4 },
                    { 7, 6 },
                    { 4, 7 },
                    { 8, 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 7, 6 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 4, 7 });

            migrationBuilder.DeleteData(
                table: "BookAuthors",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 8, 8 });

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "PublisherId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "PublisherId",
                keyValue: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
