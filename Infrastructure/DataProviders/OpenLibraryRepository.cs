using System.Text.Json;
using Domains;
using Domains.Interfaces;

namespace Infrastructure.DataProviders;

public class OpenLibraryRepository:IOpenLibraryRepository
{
    public async Task<bool> VerifyBook(Book book)
    { 
        Console.WriteLine("Verifying book in external api"+ book.coverid);
        var client = new HttpClient();
        var url =
            $"https://openlibrary.org/search.json?title=\"{book.title}\"&author_name=\"{book.author}\"&cover_i={book.coverid}&fields=cover_i,title,author_name";
        Console.WriteLine(url);
        try
        {
            var response = await client.GetAsync(url);
            Console.WriteLine("Got response + ", response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Success response");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<OpenLibraryRespone>(json);
                Console.WriteLine("Success deserialization");
                foreach (var item in data.docs)
                {
                    Console.WriteLine($"{item.title}    {item.cover_i}    {item.author_name.ToString()} {item.author_name.Length}");
                    if (item.title == book.title && item.cover_i == book.coverid &&
                        item.author_name.Contains(book.author))
                    {
                        Console.WriteLine("Found match");
                        return true;
                    }
                }
                Console.WriteLine("Not found any match");
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        return false;
    }
}