using System.Text.Json;
using BookService.Services;
using Domains;
using Domains.Interfaces;
using Domains.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BookService;

[Route("api/[controller]")]
[ApiController]
public class BookController(IBookFacade bookFacade, IMessageQueueService messageQueueService) : ControllerBase
{
    [HttpGet]
    [RequireHttps]
    public IActionResult GetAllBooks()
    {
        var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
        if (string.IsNullOrEmpty(userId)) return StatusCode(500, "User ID not provided");
        var books = bookFacade.GetAllBooks(int.Parse(userId));

        // Map domain entities to DTOs
        var bookDtos = books.Select(BookMapper.MapDTO);

        return Ok(bookDtos);
    }


    [HttpGet("{bookId}")]
    [RequireHttps]
    public async Task<IActionResult> GetBookById(int bookId)
    {
        var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
        if (string.IsNullOrEmpty(userId)) return StatusCode(500, "User ID not provided");
        var book = await bookFacade.GetBookById(int.Parse(userId), bookId);

        // Map domain entities to DTOs
        var bookDto = BookMapper.MapDTO(book);

        return Ok(bookDto);
    }

    [HttpPost]
    [RequireHttps]
    public async Task<IActionResult> AddBook(Book book)
    {
        Console.WriteLine("Im in controller");
        Console.WriteLine($"Book data {book.coverid}");
        var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
        if (string.IsNullOrEmpty(userId)) return StatusCode(500, "User ID not provided");
        Console.WriteLine("I got user id");
        try
        {
            Console.WriteLine("About to add book");
            var bookId = await bookFacade.AddBook(int.Parse(userId), book);
            //form message object
            var userBookEncrypted = new UserBookEncrypted(int.Parse(userId), bookId);
            // public message
            messageQueueService.PublishMessage("create_book", JsonSerializer.Serialize(userBookEncrypted));
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{bookId}")]
    [RequireHttps]
    public async Task<IActionResult> DeleteBook(int bookId)
    {
        var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
        if (string.IsNullOrEmpty(userId)) return StatusCode(500, "User ID not provided");

        try
        {
            await bookFacade.DeleteBook(int.Parse(userId), bookId);
            // send message to delete book for the user in progress service
            var userBookEncrypted = new UserBookEncrypted(int.Parse(userId), bookId);

            messageQueueService.PublishMessage("delete_book_queue",
                JsonSerializer.Serialize(userBookEncrypted));

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}