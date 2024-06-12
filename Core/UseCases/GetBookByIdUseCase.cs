using Domains;
using Domains.Interfaces;

namespace Core.UseCases;

public class GetBookByIdUseCase(IBookRepository bookRepository)
{
    public async Task<BookEntity> Execute(int userId, int bookId)
    {
        return await bookRepository.GetBook(userId, bookId);
    } 
    public async Task<BookMapped> Execute(GetBookRequest bookRequest)
    {
        var book = await bookRepository.GetBook(bookRequest.userId, bookRequest.bookId);
        BookMapped bookMapped = new BookMapped();
        bookMapped.Id= book.id;
        if (book.isHidden)
        { 
            bookMapped.IsHidden= true;
        }
        else
        {
            bookMapped.Title = book.title;
            bookMapped.Author = book.author;
            bookMapped.CoverId = book.coverid;
            bookMapped.IsHidden = false;
        }

        return bookMapped;
    }
   
}