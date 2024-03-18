 
using Core.UseCases;
using Domains;
using Domains.Interfaces;

namespace Core;

public class BookFacade(GetAllBooksUseCase getAllBooksUseCase) : IBookFacade
{
    public List<Book> GetAllBooks()
    {
        return getAllBooksUseCase.Execute();
    }
}