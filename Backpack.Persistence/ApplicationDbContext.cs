using Backpack.Domain.Entity;
using Backpack.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Backpack.Persistence;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Backup> Backups { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<History> Histories { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Wildcard> Wildcards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Backup entity
        modelBuilder.Entity<Backup>(b =>
        {
            // Backup has one Source Location
            b.HasOne(x => x.Source)
             .WithMany() // no navigation collection on Location
             .HasForeignKey(x => x.SourceId)
             .OnDelete(DeleteBehavior.Cascade);

            // Backup has one Destination Location
            b.HasOne(x => x.Destination)
             .WithMany()
             .HasForeignKey(x => x.DestinationId)
             .OnDelete(DeleteBehavior.Cascade);

            // Backup has many Wildcards (1-M)
            b.HasMany(x => x.Wildcards)
             .WithOne(w => w.Backup)
             .HasForeignKey(w => w.BackupId)
             .OnDelete(DeleteBehavior.Cascade);

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

        // Location entity + TPH inheritance
        modelBuilder.Entity<Location>(l =>
        {
            // TPH discriminator on LocationType
            l.HasDiscriminator<string>("LocationType")
             .HasValue<FileLocation>(eLocationType.File.ToString());
            // Add other Location subtypes here: .HasValue<FtpLocation>("Ftp"), etc.

            // Navigation properties on Location for Backups (optional, can be omitted)
            l.HasOne(x => x.SourceBackup)
             .WithMany()
             .HasForeignKey(x => x.SourceBackupId)
             .OnDelete(DeleteBehavior.Restrict);

            l.HasOne(x => x.DestinationBackup)
             .WithMany()
             .HasForeignKey(x => x.DestinationBackupId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
