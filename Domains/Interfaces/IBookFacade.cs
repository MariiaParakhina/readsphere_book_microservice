 
namespace Domains.Interfaces;

public interface IBookFacade
{
    List<BookEntity> GetAllBooks(int userId);
    Task<int> AddBook(int userId, Book book);
    Task DeleteBook(int userId, int bookId);
    Task<BookEntity> GetBookById(int userId, int bookId);
    Task<BookMapped> GetBookById(GetBookRequest getBookRequest);
    Task UpdateBookPrivacy(int userId, int bookId, bool isHidden);
}