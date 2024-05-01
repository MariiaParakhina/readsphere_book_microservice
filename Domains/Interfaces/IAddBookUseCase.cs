namespace Domains.Interfaces;

public interface IAddBookUseCase
{
    Task Execute(int userId, Book book);
}