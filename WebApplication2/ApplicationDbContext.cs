using Microsoft.EntityFrameworkCore;
using WebApplication2;
using WebApplication2.Models; // Model sınıfının bulunduğu namespace

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Users> Users { get; set; } // Users tablosu
    public DbSet<Hotels> Hotels { get; set; } // Hotels tablosu
    public DbSet<rooms> rooms { get; set; } // Rooms tablosu
    public DbSet<roomtypes> roomtypes { get; set; } // RoomTypes tablosu
    public DbSet<reservations> reservations { get; set; } // Reservations tablosu
    public DbSet<SupportRequest> SupportRequests { get; set; } // SupportRequest tablosu
    public DbSet<Employee> Employees { get; set; } // Employees tablosu


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

        // RoomTypes tablosu yapılandırması
        modelBuilder.Entity<roomtypes>().HasKey(rt => rt.id);
        modelBuilder.Entity<roomtypes>().ToTable("roomtypes");
        modelBuilder.Entity<roomtypes>()
            .Property(rt => rt.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<roomtypes>()
            .Property(rt => rt.updated_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<roomtypes>()
            .Property(rt => rt.deleted)
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

        // SupportRequest tablosu yapılandırması
        modelBuilder.Entity<SupportRequest>().HasKey(sr => sr.id); // Primary key
        modelBuilder.Entity<SupportRequest>().ToTable("support_requests"); // Tablo adı
        modelBuilder.Entity<SupportRequest>()
            .Property(sr => sr.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Varsayılan tarih
        modelBuilder.Entity<SupportRequest>()
            .Property(sr => sr.updated_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Varsayılan güncellenme tarihi
        modelBuilder.Entity<SupportRequest>()
            .Property(sr => sr.deleted)
            .HasDefaultValue(false); // Varsayılan olarak silinmiş değil
        modelBuilder.Entity<SupportRequest>()
            .HasOne(sr => sr.User)
            .WithMany(u => u.SupportRequests)
            .HasForeignKey(sr => sr.user_id)
            .OnDelete(DeleteBehavior.Cascade);

        // Reservations tablosu yapılandırması
        modelBuilder.Entity<reservations>().HasKey(r => r.id);
        modelBuilder.Entity<reservations>().ToTable("reservations");
        modelBuilder.Entity<reservations>()
            .Property(r => r.created_at)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<reservations>()
            .Property(r => r.updated_at)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Employees tablosu yapılandırması
        modelBuilder.Entity<Employee>().HasKey(e => e.id);
        modelBuilder.Entity<Employee>().ToTable("employees");

        modelBuilder.Entity<Employee>()
            .Property(e => e.created_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Employee>()
            .Property(e => e.updated_at)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Employee>()
            .Property(e => e.deleted)
            .HasDefaultValue(false);



        // Kolon isimlerinin veritabanındaki adlarla eşleşmesi için
        modelBuilder.Entity<Employee>()
            .Property(e => e.hotel_id)
            .HasColumnName("hotel_id");  // Veritabanındaki kolon ismi

        modelBuilder.Entity<Employee>()
            .Property(e => e.user_id)
            .HasColumnName("user_id");   // Veritabanındaki kolon ismi

 
    }
}

    
