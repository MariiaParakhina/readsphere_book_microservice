
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
        string userId = HttpContext.Request.Headers["X-User-Id"].ToString();
        if (userId is null) return StatusCode(500, "User ID not provided");
        List<Book> books = bookFacade.GetAllBooks(int.Parse(userId));

        // Map domain entities to DTOs
        var bookDtos = books.Select(BookMapper.MapDTO);

        return Ok(bookDtos);
    }
 
    
}