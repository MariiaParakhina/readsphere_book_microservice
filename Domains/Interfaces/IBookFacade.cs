 
namespace Domains.Interfaces;

public interface IBookFacade
{
    List<Book> GetAllBooks(int userId);
    Task AddBook(int userId, Book book);

    Task DeleteBook(int userId, int bookId);
}