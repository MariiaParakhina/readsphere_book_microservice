
namespace Domains.Interfaces;

public interface IGetAllBooksUseCase
{
    List<BookEntity> Execute(int userId);
}