using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure SQL Server connection
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDB")));

// Add MVC services
builder.Services.AddControllersWithViews();

// if (builder.Configuration == null)
// {
//     throw new InvalidOperationException("Configuration is not initialized.");
// }

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Path to redirect for login
    options.LogoutPath = "/Account/Logout"; // Path to redirect for logout
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Session expiration
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authorization:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authorization:Google:ClientSecret"];
});

// Register repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BorrowingRepository>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();

var app = builder.Build();


// Middleware pipeline setup
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enables authentication middleware
app.UseAuthorization();  // Enables authorization middleware

app.UseMiddleware<UserLoggingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();