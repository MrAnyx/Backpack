using Backpack.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Backup> Backups { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many between Backup and Profile
        modelBuilder.Entity<Backup>()
            .HasMany(b => b.Profiles)
            .WithMany(p => p.Backups)
            .UsingEntity(j => j.ToTable("BackupProfile")); // optional: customize join table name

        modelBuilder.Entity<Backup>()
            .HasOne(b => b.Source)
            .WithMany(l => l.SourceBackups)
            .HasForeignKey(b => b.SourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Backup>()
            .HasOne(b => b.Destination)
            .WithMany(l => l.DestinationBackups)
            .HasForeignKey(b => b.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
