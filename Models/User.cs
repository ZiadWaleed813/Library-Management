public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public int RoleId { get; set; } // Foreign Key to Role
    public Role? Role { get; set; } // Navigation Property
}