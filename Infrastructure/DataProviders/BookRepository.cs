using Domains;
using Domains.Interfaces;
using Infrastructure.Mappers;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;

namespace Infrastructure.DataProviders;
public class BookRepository : IBookRepository
{ 
    private readonly IDatabaseConfig _databaseConfig;
    private readonly IMemoryCache _cache;

    public BookRepository(IDatabaseConfig databaseConfig, IMemoryCache cache)
    {
        _databaseConfig = databaseConfig;
        _cache = cache;
    }
  
    public List<BookEntity> GetBooks(int userId)
    {
        if (!_cache.TryGetValue($"BooksForUser{userId}", out List<BookEntity> data))
        {
            data = new List<BookEntity>();
            var connectionString = _databaseConfig.GetConnectionString();
            var sql = "SELECT books.*, user_book.ishidden FROM books JOIN user_book ON books.Id = user_book.BookId" +
                      " WHERE user_book.UserId = @userID;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("userID", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookEntity book = BookDataMapper.map(reader);
                            data.Add(book);
                        }
                    }
                }
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Cache for 5 minutes

            _cache.Set($"BooksForUser{userId}", data, cacheEntryOptions);
        }

        return data;
    }

    // check if book exists in books db
    public async Task<int> VerifyBook(BookDTO bookDto)
    {
        var connectionString = _databaseConfig.GetConnectionString(); 
        var sql = $"SELECT Id FROM books WHERE Title=@name AND Author=@author AND CoverId=@coverID;"; 

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("coverID", bookDto.CoverId);
                cmd.Parameters.AddWithValue("name", bookDto.Title);
                cmd.Parameters.AddWithValue("author", bookDto.Author);
                int userId = (int) (await cmd.ExecuteScalarAsync() ?? 0x0);
                return userId;
            }
        }

    }

    // check if there is user with such book in db
    public async Task<bool> VerifyBook(int userId, int bookId)
    {
        var connectionString = _databaseConfig.GetConnectionString(); 
        var sql = "SELECT COUNT(*) FROM user_book WHERE userId = @userId AND bookId = @bookId;"; 

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("bookId", bookId);
                var count = (long) (await cmd.ExecuteScalarAsync() ?? 0);
                return count > 0;
            }
        }
    }

    public async Task<BookEntity> GetBook(int userId, int bookId)
    {
        var connectionString = _databaseConfig.GetConnectionString(); 
        var sql = "SELECT books.*, user_book.ishidden FROM books JOIN user_book ON books.Id = user_book.BookId" +
                  " WHERE books.Id = @bookId AND user_book.UserId = @userId;"; 

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(sql, conn))
            { 
                cmd.Parameters.AddWithValue("bookId", bookId);
                cmd.Parameters.AddWithValue("userId", userId);
                using (var reader = await cmd.ExecuteReaderAsync())
                { 
                    if (reader.Read())
                    {
                        //using mapper to the element
                        var book = BookDataMapper.map(reader);
                        Console.WriteLine(book.isHidden);
                        return book;
                    }
                    else
                    {
                        throw new Exception("No such book found for the user.");
                    }
                }
            }
        }
    }


    // add book to db return id
    public async Task<int> AddBook(BookDTO bookDto)
    {
        Console.Write("In reposirtoy adding");
        var connectionString = _databaseConfig.GetConnectionString(); 
        var sql = "INSERT INTO books (CoverId, Title, Author) VALUES (@coverId, @name, @author) RETURNING Id;"; 

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                
                cmd.Parameters.AddWithValue("coverId", bookDto.CoverId);
                cmd.Parameters.AddWithValue("name", bookDto.Title);
                cmd.Parameters.AddWithValue("author", bookDto.Author);
                var id = (int) await cmd.ExecuteScalarAsync();
                Console.Write("In reposirtoy added");
                return id;
            }
        }
    }

    // add user-book connection
    public async Task AddUserBook(int bookId, int userId)
    {
        var connectionString = _databaseConfig.GetConnectionString(); 
        var sql = "INSERT INTO user_book (UserId, BookId) VALUES (@userId, @bookId);"; 

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("bookId", bookId);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
 

    // delete book for a user
    public async Task DeleteBook(int bookId, int userId)
    {
        // delete user book connection, throw error if there is no such connection
        var connectionString = _databaseConfig.GetConnectionString(); 
        var sql = "DELETE FROM user_book WHERE UserId = @userId AND" +
                  " BookId = @bookId;"; 

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("bookId", bookId);
                var affectedRows = await cmd.ExecuteNonQueryAsync();
                
                if (affectedRows == 0)
                {
                    throw new Exception("No such connection exists between the user and the book.");
                }
            }
        }
    }

   public async Task DeleteUserData(int userId)
   {
       // Delete user-book connections
       var connectionString = _databaseConfig.GetConnectionString();  
       var sql = "DELETE FROM user_book WHERE userid = @userId;"; 
   
       using (var conn = new NpgsqlConnection(connectionString))
       {
           Console.WriteLine("Connection to delete is open");
           conn.Open();
           using (var cmd = new NpgsqlCommand(sql, conn))
           {
               cmd.Parameters.AddWithValue("userId", userId);
               await cmd.ExecuteNonQueryAsync();
           }
       }
   }

   public async Task UpdateBookPrivacy(int userId, int bookId, bool isHidden)
   {
       try
       {
           Console.WriteLine("Getting to the update privacy method");
           Console.WriteLine(isHidden);
           var connectionString = _databaseConfig.GetConnectionString();
           var sql = "UPDATE user_book SET isHidden = @isHidden WHERE UserId = @userId AND BookId = @bookId;";

           using (var conn = new NpgsqlConnection(connectionString))
           {
               conn.Open();
               using (var cmd = new NpgsqlCommand(sql, conn))
               {
                   cmd.Parameters.AddWithValue("userId", userId);
                   cmd.Parameters.AddWithValue("bookId", bookId);
                   cmd.Parameters.AddWithValue("isHidden", isHidden);
                   var affectedRows = await cmd.ExecuteNonQueryAsync();
                   Console.WriteLine($"{affectedRows} row(s) updated.");
               }
           }
       }
       catch (Exception ex)
       {
           Console.WriteLine($"An error occurred: {ex.Message}");
           throw; // rethrow the exception if you want it to be handled at a higher level
       }
   }
}