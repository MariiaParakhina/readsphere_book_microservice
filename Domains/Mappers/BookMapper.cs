namespace Domains.Mappers;

public static class BookMapper
{
    public static BookDto MapDTO(Book book)
    {
        return new BookDto()
        {
            Id = book.id,
            Author = book.author,
            CoverId = book.coverid,
            Title = book.title
        };
    }

    public static Book MapDTO(BookDto bookDto)
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