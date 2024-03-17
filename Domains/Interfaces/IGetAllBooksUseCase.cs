
namespace Domains.Interfaces;

public interface IGetAllBooksUseCase
{
    List<Book> Execute();
}