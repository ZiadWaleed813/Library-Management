using System.ComponentModel.DataAnnotations;

public class Book
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public int AuthorID { get; set; }
    public Author? Author { get; set; }
    public bool IsAvailable { get; set; } = true;
    public ICollection<Borrowing>? Borrowings { get; set; }
}
