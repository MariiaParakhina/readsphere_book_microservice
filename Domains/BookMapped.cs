namespace Domains;

public class BookMapped
{
    public int Id { get; set; } 
    public string? Title { get; set; } = null;
    public int? CoverId { get; set; } = null;
    public string? Author { get; set; } = null;
    public bool IsHidden { get; set; }
}