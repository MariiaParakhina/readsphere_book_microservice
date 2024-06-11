namespace Domains;

public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int CoverId { get; set; }
    public string Author { get; set; } 
    public bool IsHidden { get; set; }
}