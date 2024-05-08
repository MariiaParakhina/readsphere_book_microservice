using Domains;
using Npgsql;

namespace Infrastructure.Mappers;

public static class BookDataMapper
{
    public static Book map( NpgsqlDataReader reader)
    {
        return new Book
        {
            id =  (int)reader["Id"], title = (string)reader["Title"],
            author = (string)reader["Author"], coverid = (int)reader["CoverId"]
        };
    }
}