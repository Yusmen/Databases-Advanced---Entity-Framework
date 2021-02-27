namespace BookShop
{
    using BookShop.Initializer;
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Text;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {

                //DbInitializer.ResetDatabase(db);

                //var booksSelect = db.Books
                //    .SelectMany(x => x.BookCategories)
                //    .Include(x => x.Book)
                //    .ToList();

                //foreach (var item in booksSelect)
                //{
                //    Console.WriteLine(item.Book.Title);
                //}



            }
        }
        public static int RemoveBooks(BookShopContext context)
        {
            var count = context.Books.Where(x => x.Copies < 4200)
                .Delete();

            context.SaveChanges();

            return count;
        }
        public static void IncreasePrices(BookShopContext context)
        {

            context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .Update(x => new Book { Price = x.Price + 5 });

            context.SaveChanges();
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context.Categories.Select(c => new
            {
                CategotyName = c.Name,
                Books = c.CategoryBooks
                .Select(b => new
                {
                    b.Book.Title,
                    BookReleaseDate = b.Book.ReleaseDate
                })
                  .OrderByDescending(x => x.BookReleaseDate)
                  .Take(3)
                  .ToList()
            })
            .OrderBy(x => x.CategotyName)
            .ToList();


            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategotyName}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.BookReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                .Select(x => new
                {
                    x.Name,
                    Profit = x.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
                })
            .OrderByDescending(x => x.Profit)
            .ThenBy(x => x.Name)
            .ToList();

            return string.Join(Environment.NewLine, books.Select(x => $"{x.Name} {x.Profit}"));
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {

            var books = context.Authors.Select(a => new
            {
                a.FirstName,
                a.LastName,
                BooksCount = a.Books.Sum(c => c.Copies)

            })
            .OrderByDescending(x => x.BooksCount)
            .ToList();


            return string.Join(Environment.NewLine, books.Select(x => $"{x.FirstName} {x.LastName} - {x.BooksCount}"));


        }


        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Count(t => t.Title.Length > lengthCheck);
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {

            StringBuilder sb = new StringBuilder();

            var books = context.Books.Where(a => EF.Functions
              .Like(a.Author.LastName, $"{input}%"))
            .OrderBy(x => x.BookId)
            .Select(b => new
            {
                b.Title,
                AuthorName = b.Author.FirstName + " " + b.Author.LastName

            })
            .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorName})");
            }


            return sb.ToString().TrimEnd();

        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(x => new
                {
                    Name = x.FirstName + " " + x.LastName
                })
                .OrderBy(x => x.Name)
                .ToList();

            return string.Join(Environment.NewLine, authors);
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime realDate = DateTime.Parse(date);
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.ReleaseDate < realDate)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price
                })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();

        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {

            var categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();

            var books = context.Books.OrderBy(x => x.Title).Where(x => x.BookCategories
            .Any(c => categories.Contains(c.Category.Name)))
            .Select(x => x.Title)
            .ToList();


            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var sb = new StringBuilder();

            string stringDate = $"{year} - 01 - 01 00:00:00.0000000";

            DateTime date = DateTime.Parse(stringDate);

            var books = context.Books
                .Where(x => x.ReleaseDate != date)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }


            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.Price > 40)
                .Select(x => new
                {
                    x.Title,
                    x.Price
                })
                .OrderByDescending(x => x.Price)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");

            }

            return sb.ToString().TrimEnd();


        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            var bookTitles = context.Books.Where(x => x.AgeRestriction == ageRestriction)
                .OrderByDescending(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, bookTitles);
            return result;
        }
    }
}
