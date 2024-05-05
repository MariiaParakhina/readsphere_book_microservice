
    using System.Text.Json;
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
            string userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return StatusCode(500, "User ID not provided");
            List<Book> books = bookFacade.GetAllBooks(int.Parse(userId));

            // Map domain entities to DTOs
            var bookDtos = books.Select(BookMapper.MapDTO);

            return Ok(bookDtos);
        }

        [HttpPost]
        [RequireHttps]
        public async Task<IActionResult> AddBook(Book book)
        {
            string userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId.Equals(string.Empty)) return StatusCode(500, "User ID not provided");

            try
            {
                int bookId = await bookFacade.AddBook(int.Parse(userId), book);
                //form message object
                UserBookEncrypted userBookEncrypted = new UserBookEncrypted(int.Parse(userId), bookId);
                // public message
                messageQueueService.PublishMessage("create_book", JsonSerializer.Serialize(userBookEncrypted));
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{bookId}")]
        [RequireHttps]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            string userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return StatusCode(500, "User ID not provided");

            try
            {
                await bookFacade.DeleteBook(int.Parse(userId), bookId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }