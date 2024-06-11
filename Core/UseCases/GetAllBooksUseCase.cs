 using Domains;
using Domains.Interfaces;
using Infrastructure.DataProviders;

namespace Core.UseCases;

public class GetAllBooksUseCase:IGetAllBooksUseCase
{
    private readonly IBookRepository _bookRepository;
    
    public GetAllBooksUseCase(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public List<BookEntity> Execute(int userId)
    {
        return _bookRepository.GetBooks(userId);
    }
}