using System.Reflection;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class BookDbContext:DbContext, IBookDbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options):base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<UserBook> UserBooks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly()
        );

        base.OnModelCreating(modelBuilder);
    }
     
}