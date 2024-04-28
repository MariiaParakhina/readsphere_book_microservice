 
using Core.UseCases;
using Domains;
using Domains.Interfaces;

namespace Core;

public class BookFacade: IBookFacade
{
    private readonly GetAllBooksUseCase _getAllBooksUseCase;

    public BookFacade(GetAllBooksUseCase getAllBooksUseCase)
    {
        _getAllBooksUseCase = getAllBooksUseCase;
    }
    public List<Book> GetAllBooks(int userId)
    {
        return _getAllBooksUseCase.Execute(userId);
    }
}