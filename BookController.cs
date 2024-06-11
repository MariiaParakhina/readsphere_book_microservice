
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
            string userIdStr = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userIdStr is null) return StatusCode(500, "User ID not provided");
            int userId = int.Parse(userIdStr);
            List<BookEntity> books = bookFacade.GetAllBooks(userId);

            // Map domain entities to DTOs
            var bookDtos = books.Select(BookMapper.MapDTO);

            return Ok(bookDtos);
        }
        
        
        [HttpGet("{bookId}")]
        [RequireHttps]
        public async Task< IActionResult> GetBookById(int bookId)
        {
            bookMetrics.AddRequest();
            string userIdStr = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userIdStr is null) return StatusCode(500, "User ID not provided");
            int userId = int.Parse(userIdStr);
            BookEntity book = await bookFacade.GetBookById(userId, bookId);
            Console.WriteLine(book.isHidden);
            // Map domain entities to DTOs
            BookDTO bookDto =  BookMapper.MapDTO(book);

            return Ok(bookDto);
        }

        [HttpPost]
        [RequireHttps]
        public async Task<IActionResult> AddBook(Book book)
        {
            bookMetrics.AddRequest();
            string userIdStr = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userIdStr is null) return StatusCode(500, "User ID not provided");
            int userId = int.Parse(userIdStr); 
            try
            {
                Console.WriteLine("About to add book");
                int bookId = await bookFacade.AddBook(userId, book);
                //form message object
                UserBookEncrypted userBookEncrypted = new UserBookEncrypted(userId, bookId);
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
            string userIdStr = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userIdStr is null) return StatusCode(500, "User ID not provided");
            int userId = int.Parse(userIdStr);

            try
            {
                
                await bookFacade.DeleteBook(userId, bookId);
                // send message to delete book for the user in progress service
                UserBookEncrypted userBookEncrypted = new UserBookEncrypted(userId, bookId);
                
                messageQueueService.PublishMessage("delete_book_queue", 
                    JsonSerializer.Serialize(userBookEncrypted));
                
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{bookId}")]
        [RequireHttps]
        public async Task<IActionResult> UpdateBookPrivacy(UpdateBookPrivacyRequest updateBookPrivacyRequest, int bookId)
        {
            bookMetrics.AddRequest();
            string userIdStr = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userIdStr is null) return StatusCode(500, "User ID not provided");
            int userId = int.Parse(userIdStr);
            try
            { 
                await bookFacade.UpdateBookPrivacy(userId, bookId, updateBookPrivacyRequest.IsHidden); 
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }