 
namespace Domains.Interfaces;

public interface IBookRepository
{
    List<BookEntity> GetBooks(int userId);

    Task<int> VerifyBook(BookDTO bookDto);
    Task<bool> VerifyBook(int userId, int bookId);
    Task<BookEntity> GetBook(int userId, int bookId);

    Task<int> AddBook(BookDTO bookDto);
    Task AddUserBook(int bookId, int userId);
    Task DeleteBook(int bookId, int userId);

    Task DeleteUserData(int userId);
    Task UpdateBookPrivacy(int userId, int bookId, bool isHidden);

}