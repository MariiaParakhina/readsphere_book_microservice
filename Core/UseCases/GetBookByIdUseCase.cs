using Domains;
using Domains.Interfaces;

namespace Core.UseCases;

public class GetBookByIdUseCase(IBookRepository bookRepository)
{
    public async Task<BookEntity> Execute(int userId, int bookId)
    {
        return await bookRepository.GetBook(userId, bookId);
    }
}