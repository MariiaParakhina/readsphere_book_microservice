using Domains;
using Domains.Interfaces;

namespace Core.UseCases;

public class GetBookByIdUseCase(IBookRepository bookRepository)
{
    public async Task<Book> Execute(int userId, int bookId)
    {
        return await bookRepository.GetBook(userId, bookId);
    }
}