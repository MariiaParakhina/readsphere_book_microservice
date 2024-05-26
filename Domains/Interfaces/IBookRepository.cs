namespace Domains.Interfaces;

public interface IBookRepository
{
    List<Book> GetBooks(int userId);

    Task<int?> VerifyBook(BookDto bookDto);
    Task<bool> VerifyBook(int userId, int bookId);
    Task<Book> GetBook(int userId, int bookId);

    Task<int?> AddBook(BookDto bookDto);
    Task AddUserBook(int bookId, int userId);
    Task DeleteBook(int bookId, int userId);

    Task DeleteUserData(int userId);
}