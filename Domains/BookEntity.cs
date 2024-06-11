namespace Domains;

public class BookEntity
{
    public int id { get; set; }
    public string title { get; set; }
    public int coverid { get; set; }
    public string author { get; set; } 
    public bool isHidden { get; set; }
}