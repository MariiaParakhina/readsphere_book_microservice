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
        var sql = "SELECT books.* FROM books JOIN user_book ON books.id = user_book.book_id WHERE user_book.user_id = @userID;"; 
 
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
}