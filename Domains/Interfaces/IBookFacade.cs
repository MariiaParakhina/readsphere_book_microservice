 
namespace Domains.Interfaces;

public interface IBookFacade
{
    List<Book> GetAllBooks(int userId);
    Task<int> AddBook(int userId, Book book);

    Task DeleteBook(int userId, int bookId);
    Task<Book> GetBookById(int userId, int bookId);
}