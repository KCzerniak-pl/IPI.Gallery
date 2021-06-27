using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    // Klasa kontekstu połączenia z bazą danych.
    // Wymaga dodatkowych bibliotek: Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.SqlServer.
    public class GalleryDbContext : DbContext
    {
        // Ukrycie domyślnego konstruktora.
        protected GalleryDbContext() { }

        public GalleryDbContext(DbContextOptions<GalleryDbContext> options) : base(options) { }

        // Właściwość DbSet dla encji, która jest mapowana na tabele i widoki bazy danych.
        // DbSet reprezentuje zestaw encji, których można używać do operacji tworzenia, odczytu, aktualizacji i usuwania.
        public virtual DbSet<Photo> Photos { get; set; }
    }
}
