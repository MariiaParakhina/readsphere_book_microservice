using Domains;
using Npgsql;

namespace Infrastructure.Mappers;

public static class BookDataMapper
{
    public static Book map( NpgsqlDataReader reader)
    {
        return new Book
        {
            Id =  (int)reader["book_id"], Title = (string)reader["title"],
            Author = (string)reader["author_name"], CoverId = (int)reader["cover_id"]
        };
    }
}