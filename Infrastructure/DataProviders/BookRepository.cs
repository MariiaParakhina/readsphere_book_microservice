using Domains;
using Domains.Interfaces;
using Infrastructure.Mappers;
using Npgsql;

namespace Infrastructure.DataProviders;
public class BookRepository(IDatabaseConfig databaseConfig) : IBookRepository
{
    public List<Book> GetBooks()
    {
        List<Book> data = new List<Book>();
        var connectionString = databaseConfig.GetConnectionString(); 
        var sql = "SELECT * FROM book";

        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();
 
        using (var cmd = new NpgsqlCommand(sql, conn))
        {
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

        return data;
    }
}