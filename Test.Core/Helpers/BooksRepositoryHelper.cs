using Domains;
using Domains.Interfaces;

namespace Test.Core.Helpers;

public class BooksRepositoryHelper: IBookRepository
{
    public List<Book> GetBooks()
    {
        return new List<Book> { new Book { Author = "Author", CoverId = 0001, Id = 1, Title = "title" } };

    }
}