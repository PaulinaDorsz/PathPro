using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PathPro.Data
{
    // Klasa AuthDbContext dziedziczy po IdentityDbContext
    public class PathProAuthDbContext : IdentityDbContext
    {
        // Konstruktor przyjmuje opcje DbContextOptions<AuthDbContext>
        public PathProAuthDbContext(DbContextOptions<PathProAuthDbContext> options) : base(options)
        {
        }

        // Metoda OnModelCreating, nadpisuje metodę bazową
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Wywołanie metody bazowej

            // Definicja ról w systemie
            var readerRoleId = "a71a55d6-99d7-4123-b4e0-1218ecb90e3e";
            var writerRoleId = "c309fa92-2123-47be-b397-a1c77adb502c";

            // Lista ról do dodania
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            // Dodanie ról do kontekstu bazy danych
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}

