public class BookDetailsViewModel
{
    public Book Book { get; set; } = null!;
    public bool IsBorrowed { get; set; } // Computed property for borrow status
    public int? BorrowingId { get; set; } // For returning the book if borrowed 
}