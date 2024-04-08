
using Domains;
using Domains.Interfaces;
using Domains.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BookService;

[Route("api/[controller]")]
[ApiController]
public class BookController(IBookFacade bookFacade) : ControllerBase
{
    // [HttpGet]
    [RequireHttps]
    public IActionResult GetAllBooks()
    {
        List<Book> books = bookFacade.GetAllBooks();

        // Map domain entities to DTOs
        var bookDtos = books.Select(BookMapper.MapDTO);

        return Ok(bookDtos);
    }
 
    
}