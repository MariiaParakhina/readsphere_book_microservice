namespace Domains.Mappers;

public static class BookMapper
{
    public static BookDTO MapDTO(BookEntity book)
    {
        return new BookDTO()
        {
            Id = book.id,
            Author = book.author,
            CoverId = book.coverid,
            Title = book.title,
            IsHidden = book.isHidden
        };
    }
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
    public static BookEntity MapDTO(BookDTO bookDto)
    {
        return new BookEntity()
        {
            id = bookDto.Id,
            author = bookDto.Author,
            coverid = bookDto.CoverId,
            title = bookDto.Title
        };
    }
    
}