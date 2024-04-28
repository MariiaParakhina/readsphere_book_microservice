using Domains;
using Npgsql;

namespace Infrastructure.Mappers;

public static class BookDataMapper
{
    public static Book map( NpgsqlDataReader reader)
    {
        return new Book
        {
            ID =  (int)reader["id"], Title = (string)reader["name"],
            Author = (string)reader["author"], Cover_ID = (int)reader["cover_id"]
        };
    }
}