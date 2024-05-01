using Domains;
using Domains.Interfaces;
using Domains.Mappers;
using Infrastructure.Mappers;

namespace Core.UseCases;

public class AddBookUseCase(IBookRepository bookRepository, IOpenLibraryRepository openLibraryRepository) : IAddBookUseCase
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IOpenLibraryRepository _openLibraryRepository = openLibraryRepository;


    public async Task Execute(int userId, Book book)
    {
        // verify that book exists in external api
        bool isBookExists = await _openLibraryRepository.VerifyBook(book);
        if (!isBookExists) throw new Exception("Book does not exists originally");
        
        // check if book in books
        BookDTO bookDto = BookMapper.MapDTO(book);
        int  bookId = await _bookRepository.VerifyBook(bookDto); 
        if (bookId == 0)
        {
            bookId = await  _bookRepository.AddBook(bookDto);
        }
        // check user book connection
        bool isExistingConnection = await _bookRepository.VerifyBook(userId, bookId);
        if (isExistingConnection) throw new Exception("User already has added this book");
        await _bookRepository.AddUserBook(bookId, userId);
    }
}