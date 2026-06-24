using Microsoft.EntityFrameworkCore;
using task04UserManagement.Models;

namespace task04UserManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Force PostgreSQL compatible column types
            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasColumnType("text")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnType("text")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnType("text")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Status)
                .HasColumnType("text")
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.RegistrationTime)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<User>()
                .Property(u => u.LastLoginTime)
                .HasColumnType("timestamp with time zone");

            base.OnModelCreating(modelBuilder);
        }
    }
}