using Domains;
using Domains.Interfaces;
using Domains.Mappers;
using Infrastructure.Mappers;

namespace Core.UseCases;

public class AddBookUseCase(IBookRepository bookRepository, IOpenLibraryRepository openLibraryRepository) : IAddBookUseCase
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IOpenLibraryRepository _openLibraryRepository = openLibraryRepository;


    public async Task<int> Execute(int userId, Book book)
    {
        Console.WriteLine("Checking if book exists");
        // verify that book exists in external api
        bool isBookExists = await _openLibraryRepository.VerifyBook(book);
        if (!isBookExists) throw new Exception("Book does not exists originally");
        
        // check if book in books
        BookDTO bookDto = BookMapper.MapDTO(book);
        int  bookId = await _bookRepository.VerifyBook(bookDto); 
        if (bookId == 0)
        {
            Console.WriteLine("Adding book");
            bookId = await  _bookRepository.AddBook(bookDto);
        }
        Console.WriteLine("Added book");
        // check user book connection
        bool isExistingConnection = await _bookRepository.VerifyBook(userId, bookId);
        if (isExistingConnection) throw new Exception("User already has added this book");
        await _bookRepository.AddUserBook(bookId, userId);
        return bookId;
    }
}