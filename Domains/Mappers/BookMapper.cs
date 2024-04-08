namespace Domains.Mappers;

public static class BookMapper
{
    public static BookDTO MapDTO(Book book)
    {
        return new BookDTO()
        {
            ID = book.ID,
            Author = book.Author,
            Cover_ID = book.Cover_ID,
            Title = book.Title
        };
    }

    public static Book MapDTO(BookDTO bookDto)
    {
        return new Book()
        {
            ID = bookDto.ID,
            Author = bookDto.Author,
            Cover_ID = bookDto.Cover_ID,
            Title = bookDto.Title
        };
    }
    
}