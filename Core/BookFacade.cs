 
using Core.UseCases;
using Domains;
using Domains.Interfaces;

namespace Core;

public class BookFacade(
    GetAllBooksUseCase getAllBooksUseCase,
    AddBookUseCase addBookUseCase,
    DeleteBookUseCase deleteBookUseCase,
    GetBookByIdUseCase getBookByIdUseCase)
    : IBookFacade
{
    public List<Book> GetAllBooks(int userId)
    {
        return getAllBooksUseCase.Execute(userId);
    }

    public async Task<int> AddBook(int userId, Book book)
    {
        return await addBookUseCase.Execute(userId, book);
    }

    public async Task DeleteBook(int userId, int bookId)
    {
        await deleteBookUseCase.Execute(userId, bookId);
    }

    public async Task<Book> GetBookById(int userId, int bookId)
    {
        return await getBookByIdUseCase.Execute(userId, bookId);
    }
}