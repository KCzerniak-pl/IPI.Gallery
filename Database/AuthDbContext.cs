using Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    // Klasa kontekstu połączenia z bazą danych (dla kont użytkowników).
    // Wymaga dodatkowych bibliotek: Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.SqlServer, Microsoft.AspNetCore.Identity.EntityFrameworkCore.
    public class AuthDbContext : IdentityDbContext<GalleryUser>
    {
        // Ukrycie domyślnego konstruktora.
        protected AuthDbContext() { }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    }
}
