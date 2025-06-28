using Backpack.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Test> Tests { get; set; }
}
