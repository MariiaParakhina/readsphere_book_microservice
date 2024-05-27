namespace Domains.Mappers;

public static class BookMapper
{
    public static BookDTO MapDTO(Book book)
    {
        return new BookDTO()
        {
            Id = book.id,
            Author = book.author,
            CoverId = book.coverid,
            Title = book.title
        };
    }

    public static Book MapDTO(BookDTO bookDto)
    {
        return new Book()
        {
            id = bookDto.Id,
            author = bookDto.Author,
            coverid = bookDto.CoverId,
            title = bookDto.Title
        };
    }
    
}