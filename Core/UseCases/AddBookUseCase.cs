using Domains;
using Domains.Interfaces;
using Domains.Mappers;

namespace Core.UseCases;

public class AddBookUseCase(IBookRepository bookRepository, IOpenLibraryRepository openLibraryRepository)
    : IAddBookUseCase
{

    public async Task<int> Execute(int userId, Book book)
    { 
        // verify that book exists in external api
        var isBookExists = await openLibraryRepository.VerifyBook(book);
        if (!isBookExists) throw new Exception("Book does not exists originally");

        // check if book in books
        var bookDto = BookMapper.MapDTO(book);
        int bookId = (int)(await bookRepository.VerifyBook(bookDto))!;
        if (bookId == 0)
        { 
            bookId = (int)(await bookRepository.AddBook(bookDto))!;
        }
 
        // check user book connection
        var isExistingConnection = await bookRepository.VerifyBook(userId, bookId);
        if (isExistingConnection) throw new Exception("User already has added this book");
        await bookRepository.AddUserBook(bookId, userId);
        return bookId;
    }
}