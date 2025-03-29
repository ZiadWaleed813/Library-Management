using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AuthorsController : Controller
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorsController(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public IActionResult Index()
    {
        var authors = _authorRepository.GetAllAuthors();
        return View(authors);
    }

    public IActionResult Details(int id)
    {
        var author = _authorRepository.GetAuthorById(id);
        if (author == null) return NotFound();

        return View(author);
    }

    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Create(Author author)
    {
        if (ModelState.IsValid && _authorRepository.AddAuthor(author))
        {
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Edit(int id)
    {
        var author = _authorRepository.GetAuthorById(id);
        if (author == null) return NotFound();

        return View(author);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Edit(Author author)
    {
        if (ModelState.IsValid && _authorRepository.UpdateAuthor(author))
        {
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    [Authorize(Roles = "User,Admin")]
    public IActionResult Delete(int id)
    {
        if (_authorRepository.DeleteAuthor(id))
        {
            return RedirectToAction(nameof(Index));
        }
        return NotFound();
    }
}
