using System.Text.Json;
using Domains;
using Domains.Interfaces;

namespace Infrastructure.DataProviders;

public class OpenLibraryRepository:IOpenLibraryRepository
{
    public async Task<bool> VerifyBook(Book book)
    { 
        var client = new HttpClient();
        var url =
            $"https://openlibrary.org/search.json?title={book.title}&author_name={book.author}&cover_i={book.coverid}&fields=cover_i,title,author_name";
        var response = await client.GetAsync(url);
      
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<OpenLibraryRespone>(json);
            foreach (var item in data.docs)
            {
                if (item.title == book.title && item.cover_i == book.coverid && item.author_name.Contains(book.author))
                {
                    return true;
                }
            }
        }
        
        return false; 
    }
}