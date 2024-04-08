using System.Globalization;
using Domains;
using Domains.Interfaces;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.DataProviders;
public class BookRepository : IBookRepository
{ 
    private readonly IDatabaseConfig _databaseConfig;

    public BookRepository(IDatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }
    public List<Book> GetBooks()
    {
        List<Book> data = new List<Book>();
        var connectionString = _databaseConfig.GetConnectionString(); 
        var sql = "SELECT * FROM book"; 
 
        using (var conn = new NpgsqlConnection(connectionString))
        {
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
        }
        return data;
    }
}