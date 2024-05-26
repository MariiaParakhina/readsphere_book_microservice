using Domains;
using Domains.Interfaces;

namespace Core.UseCases;

public class GetAllBooksUseCase : IGetAllBooksUseCase
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public List<Book> Execute(int userId)
    {
        return _bookRepository.GetBooks(userId);
    }
}