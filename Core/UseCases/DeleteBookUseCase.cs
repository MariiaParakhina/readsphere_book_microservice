using Domains.Interfaces;

namespace Core.UseCases;

public class DeleteBookUseCase(IBookRepository bookRepository) : IDeleteBookUseCase
{
    private readonly IBookRepository _bookRepository = bookRepository;

    public async Task Execute(int userId, int bookId)
    {
        await _bookRepository.DeleteBook(bookId, userId);
    }
}