namespace Domains.Interfaces;

public interface IAddBookUseCase
{
    Task<int> Execute(int userId, Book book);
}