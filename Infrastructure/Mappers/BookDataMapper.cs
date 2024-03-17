using Domains;
using Npgsql;

namespace Infrastructure.Mappers;

public static class BookDataMapper
{
    public static Book map( NpgsqlDataReader reader)
    {
        return new Book
        {
            ID =  (int)reader["book_id"], Title = (string)reader["title"],
            Author = (string)reader["author_name"], Cover_ID = (int)reader["cover_id"]
        };
    }
}