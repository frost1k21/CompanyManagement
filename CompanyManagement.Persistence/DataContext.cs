using CompanyManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagement.Persistence;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Title)
            .IsUnique();

        modelBuilder.Entity<Position>()
            .HasIndex(p => p.Title)
            .IsUnique();
    }
}