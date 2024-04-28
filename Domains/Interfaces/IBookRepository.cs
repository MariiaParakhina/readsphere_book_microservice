 
namespace Domains.Interfaces;

public interface IBookRepository
{
    List<Book> GetBooks(int userId);
}