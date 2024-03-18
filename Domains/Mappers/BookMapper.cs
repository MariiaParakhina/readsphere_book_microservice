namespace Domains.Mappers;

public static class BookMapper
{
    public static BookDTO MapDTO(Book book)
    {
        return new BookDTO()
        {
            Id = book.Id,
            Author = book.Author,
            CoverId = book.CoverId,
            Title = book.Title
        };
    }

    public static Book MapDTO(BookDTO bookDto)
    {
        return new Book()
        {
            Id = bookDto.Id,
            Author = bookDto.Author,
            CoverId = bookDto.CoverId,
            Title = bookDto.Title
        };
    }
    
}