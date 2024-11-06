using Microsoft.EntityFrameworkCore;
using WebApplication2;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Users> Users { get; set; } // Users tablosu
    public DbSet<Hotels> Hotels { get; set; } // Hotels tablosu

    public DbSet<rooms> rooms { get; set; }
    public DbSet<roomtypes> roomtypes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users tablosu yapılandırması
        modelBuilder.Entity<Users>().HasKey(u => u.id);
        modelBuilder.Entity<Users>().ToTable("users");
        modelBuilder.Entity<Users>()
            .HasIndex(u => u.email)
            .IsUnique();

        // Hotels tablosu yapılandırması
        modelBuilder.Entity<Hotels>().HasKey(h => h.id);
        modelBuilder.Entity<Hotels>().ToTable("hotels");
        modelBuilder.Entity<Hotels>()
            .Property(h => h.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Hotels>()
            .Property(h => h.updated_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Hotels>()
            .Property(h => h.deleted)
            .HasDefaultValue(false);

        // Rooms tablosu yapılandırması
        modelBuilder.Entity<rooms>().HasKey(r => r.id);
        modelBuilder.Entity<rooms>().ToTable("rooms");

        modelBuilder.Entity<rooms>()
            .Property(r => r.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<rooms>()
            .Property(r => r.updated_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<rooms>()
            .Property(r => r.deleted)
            .HasDefaultValue(false);

        // Foreign key ilişkileri
        modelBuilder.Entity<rooms>()
            .HasOne(r => r.hotel)
            .WithMany()
            .HasForeignKey(r => r.hotel_id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<rooms>()
            .HasOne(r => r.roomType)
            .WithMany()
            .HasForeignKey(r => r.room_type_id)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
