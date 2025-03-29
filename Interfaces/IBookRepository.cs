public interface IBookRepository
{
    IEnumerable<Book> GetAllBooks();
    Book? GetBookById(int id);
    bool AddBook(Book book);
    bool UpdateBook(Book book);
    bool DeleteBook(int id);
}