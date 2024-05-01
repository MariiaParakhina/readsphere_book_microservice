 
namespace Domains.Interfaces;

public interface IBookRepository
{
    List<Book> GetBooks(int userId);

    Task<int> VerifyBook(BookDTO bookDto);
    Task<bool> VerifyBook(int userId, int bookId);

    Task<int> AddBook(BookDTO bookDto);
    Task AddUserBook(int bookId, int userId);
    Task DeleteBook(int bookId, int userId);
    
}