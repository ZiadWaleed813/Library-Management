public class Role
{
    public int Id { get; set; }
    public string? Name { get; set; } // E.g., Admin, Librarian, User
    public ICollection<User>? Users { get; set; } // Navigation Property
}