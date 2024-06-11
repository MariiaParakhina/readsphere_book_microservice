 
using Core.UseCases;
using Domains;
using Domains.Interfaces;

namespace Core;

public class BookFacade(
    GetAllBooksUseCase getAllBooksUseCase,
    AddBookUseCase addBookUseCase,
    DeleteBookUseCase deleteBookUseCase,
    GetBookByIdUseCase getBookByIdUseCase,
    UpdateBookPrivacyUseCase updateBookPrivacyUseCase)
    : IBookFacade
{
    public List<BookEntity> GetAllBooks(int userId)
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

    public async Task<BookEntity> GetBookById(int userId, int bookId)
    {
        return await getBookByIdUseCase.Execute(userId, bookId);
    }

    public async Task UpdateBookPrivacy(int userId, int bookId, bool isHidden)
    {
        await updateBookPrivacyUseCase.Execute(userId, bookId, isHidden);
    }
}