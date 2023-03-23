using Microsoft.EntityFrameworkCore;

namespace Demo.Models;

public class EfDbContext : DbContext
{
    public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
    {
    }

    public DbSet<Logs>? Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Logs>().HasKey(x => x.Id);
        modelBuilder.Entity<Logs>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

    }
}