using Backpack.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Test> Tests { get; set; }
}
