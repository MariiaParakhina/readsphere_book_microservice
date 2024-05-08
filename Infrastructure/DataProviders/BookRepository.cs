using Domains;
using Domains.Interfaces;
using Infrastructure.Mappers; 
using Npgsql;

namespace Infrastructure.DataProviders;
public class BookRepository : IBookRepository
{ 
    private readonly IDatabaseConfig _databaseConfig;

    public BookRepository(IDatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }
    public List<Book> GetBooks(int userId)
    {
        List<Book> data = new List<Book>();
        var connectionString = _databaseConfig.GetConnectionString(); 
        var sql = "SELECT books.* FROM books JOIN user_book ON books.Id = user_book.BookId" +
                  " WHERE user_book.UserId = @userID;"; 
   Console.WriteLine(connectionString);
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
                       //using mapper to the element
                       var book = BookDataMapper.map(reader);
                       data.Add(book);
                    }
                }
            }
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

    // add book to db return id
    public async Task<int> AddBook(BookDTO bookDto)
    {
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
        var sql = "DELETE FROM user_book WHERE UserId = @userId AND BookId = @bookId;"; 

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
}