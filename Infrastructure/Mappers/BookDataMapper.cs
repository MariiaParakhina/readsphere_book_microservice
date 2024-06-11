using Domains;
using Npgsql;

namespace Infrastructure.Mappers;

public static class BookDataMapper
{
    // public static Book map( NpgsqlDataReader reader)
    // {
    //     return new Book
    //     {
    //         id =  (int)reader["Id"], title = (string)reader["Title"],
    //         author = (string)reader["Author"], coverid = (int)reader["CoverId"]
    //     };
    // }
    public static BookEntity map(NpgsqlDataReader reader)
    {
        Console.WriteLine(reader.ToString());
        Console.WriteLine(reader["ishidden"]);
        return new BookEntity
        {
            id = Convert.ToInt32(reader["id"]),
            title = Convert.ToString(reader["title"])!,
            coverid = Convert.ToInt32(reader["coverid"]),
            author = Convert.ToString(reader["author"])!,
            isHidden = Convert.ToBoolean(reader["ishidden"])
        };
    }
}