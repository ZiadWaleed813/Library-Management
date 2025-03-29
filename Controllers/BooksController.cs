using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class BooksController : Controller
{
    private readonly LibraryContext _context;
    private readonly BookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly BorrowingRepository _borrowingRepository; // Added for borrowing functionality

    public BooksController(LibraryContext context, BookRepository bookRepository, BorrowingRepository borrowingRepository, IAuthorRepository authorRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        _borrowingRepository = borrowingRepository ?? throw new ArgumentNullException(nameof(borrowingRepository));
        _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
    }

    public IActionResult Index()
    {
        var books = _bookRepository.GetAllBooks().ToList();

        return View(books);
    }

    public IActionResult Details(int id)
    {
        var book = _bookRepository.GetBookById(id);

        if (book == null)
        {
            return RedirectToAction("NotFound", "Books");
        }

        var borrowingRecord = _context.Borrowings
        .Include(b => b.Book)
        .FirstOrDefault(b => b.BookId == id && b.Book != null && !b.Book.IsAvailable);

        var viewModel = new BookDetailsViewModel
        {
            Book = book,
            IsBorrowed = borrowingRecord != null,
            BorrowingId = borrowingRecord?.Id
        };

        return View(viewModel);
    }


    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Create()
    {
        // var authors = _context.Authors.ToList(); // Fetch all authors from the database
        var authors = _authorRepository.GetAllAuthors().ToList();

        ViewBag.Authors = new SelectList(authors, "ID", "Name"); // Create a dropdown list
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            bool success = _bookRepository.AddBook(book);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Failed to add the book.");
        }
        return View(book);
    }

    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Edit(int id)
    {
        var book = _bookRepository.GetBookById(id);
        if (book == null) return NotFound();

        ViewBag.Authors = new SelectList(_context.Authors, "ID", "Name");
        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Edit(Book book)
    {
        if (ModelState.IsValid && _bookRepository.UpdateBook(book))
        {
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Authors = new SelectList(_context.Authors, "ID", "Name");
        return View(book);
    }

    [Authorize(Roles = "User,Admin")]
    public IActionResult Delete(int id)
    {
        if (_bookRepository.DeleteBook(id))
        {
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }

    private int? GetUserId()
    {
        // Assuming you're using authentication middleware and User.Claims
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
    }

    public IActionResult NotFound()
    {
        return View();
    }
}