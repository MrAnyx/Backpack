using Backpack.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Backup> Backups { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<History> Histories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Backup entity
        modelBuilder.Entity<Backup>(b =>
        {
            // Many-to-many Backup <-> Profile
            b.HasMany(x => x.Profiles)
             .WithMany(p => p.Backups)
             .UsingEntity(j => j.ToTable("BackupProfile"));
        });

        // Profile entity
        modelBuilder.Entity<Profile>(p =>
        {
            // Profile has many Histories (1-M)
            p.HasMany(x => x.Histories)
             .WithOne(h => h.Profile)
             .HasForeignKey(h => h.ProfileId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
