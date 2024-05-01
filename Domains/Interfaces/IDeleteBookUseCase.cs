namespace Domains.Interfaces;

public interface IDeleteBookUseCase
{
    Task Execute(int userId, int bookId);
}