using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.IO;
using Utils.Constants;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });

            modelBuilder.Entity<User>()
            .HasData(new User
            {
                Id = Guid.Parse("78f5610c-77be-45a7-be07-effb2bab65ff"),
                UserName = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FullName = "Quản trị viên",
                Email = "admin@test.tt",
                PhoneNumber = "0987654321",
                Roles = Roles.Admin,
                CreatedAt = DateTime.Now,
                CreatedBy = "Seeding"
            });




            base.OnModelCreating(modelBuilder);
        }
    }
}