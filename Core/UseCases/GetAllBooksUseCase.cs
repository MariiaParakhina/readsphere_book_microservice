 
using Domains;
using Domains.Interfaces;

namespace Core.UseCases;

public class GetAllBooksUseCase(IBookRepository bookRepository) : IGetAllBooksUseCase
{
    public List<Book> Execute()
    {
        return bookRepository.GetBooks();
    }
}