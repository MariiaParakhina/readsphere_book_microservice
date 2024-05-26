using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure;

public interface IBookDbContext
{
    DbSet<Book> Books { get; set; }
    DbSet<UserBook> UserBooks { get; set; }

    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync
        (CancellationToken cancellationToken = default);
}