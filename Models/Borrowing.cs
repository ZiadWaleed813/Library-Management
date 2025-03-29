public class Borrowing
{
    public int Id { get; set; }
    public int UserId { get; set; } // Foreign Key to User
    public User? User { get; set; } // Navigation Property

    public int BookId { get; set; } // Foreign Key to Book
    public Book? Book { get; set; } // Navigation Property

    public DateTime? BorrowedDate { get; set; }
}