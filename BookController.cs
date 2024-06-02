
    using System.Text.Json;
    using Domains;
    using Domains.Interfaces;
    using Domains.Mappers;
    using Microsoft.AspNetCore.Mvc;
    using Prometheus;

    namespace BookService;

    [Route("api/[controller]")]
    [ApiController]
    public class BookController(IBookFacade bookFacade, IMessageQueueService messageQueueService, IBookMetrics bookMetrics) : ControllerBase
    {
        [HttpGet]
        [RequireHttps]
        public IActionResult GetAllBooks()
        {
            bookMetrics.AddRequest();
            string userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return StatusCode(500, "User ID not provided");
            List<Book> books = bookFacade.GetAllBooks(int.Parse(userId));

            // Map domain entities to DTOs
            var bookDtos = books.Select(BookMapper.MapDTO);

            return Ok(bookDtos);
        }
        
        
        [HttpGet("{bookId}")]
        [RequireHttps]
        public async Task< IActionResult> GetBookById(int bookId)
        {
            bookMetrics.AddRequest();
            string userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return StatusCode(500, "User ID not provided");
            Book book = await bookFacade.GetBookById(int.Parse(userId), bookId);

            // Map domain entities to DTOs
            var bookDto =  BookMapper.MapDTO(book);

            return Ok(bookDto);
        }

        [HttpPost]
        [RequireHttps]
        public async Task<IActionResult> AddBook(Book book)
        {
            bookMetrics.AddRequest();
            string userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId.Equals(string.Empty)) return StatusCode(500, "User ID not provided");
            Console.WriteLine("I got user id");
            try
            {
                Console.WriteLine("About to add book");
                int bookId = await bookFacade.AddBook(int.Parse(userId), book);
                //form message object
                UserBookEncrypted userBookEncrypted = new UserBookEncrypted(int.Parse(userId), bookId);
                // public message
                messageQueueService.PublishMessage("create_book", JsonSerializer.Serialize(userBookEncrypted));
                return Ok(bookId);
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
            bookMetrics.AddRequest();
            string userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return StatusCode(500, "User ID not provided");

            try
            {
                
                await bookFacade.DeleteBook(int.Parse(userId), bookId);
                // send message to delete book for the user in progress service
                UserBookEncrypted userBookEncrypted = new UserBookEncrypted(int.Parse(userId), bookId);
                
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