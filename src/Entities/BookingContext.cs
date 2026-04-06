using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Entities;

public class BookingContext : DbContext
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public BookingContext(DbContextOptions<BookingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // It might be nice to also specify some check constraints, but that doesn't seem to come out of
        // the box with EF. See https://github.com/efcore/EFCore.CheckConstraints for a possible add-on.

        // A couple of tables involve auto-generated identity columns. The only reason for using different
        // seed values is to make it a bit more obvious what I'm looking at when browsing the database
        // (4 digit numbers are hotels, 6 digit numbers are rooms)

        // The index on the hotel name supports the lookup by name. The main thing is to impose a
        // unique constraint (it's likely not a performance thing, because I don't expect to have a giant
        // number of hotels)

        var hotels = modelBuilder.Entity<Hotel>();
        hotels.HasKey(h => h.HotelId);
        hotels.Property(h => h.HotelId).ValueGeneratedOnAdd().UseIdentityColumn(1000);
        hotels
            .HasMany(h => h.Rooms)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId);
        hotels.Property(h => h.Name).HasMaxLength(100);
        hotels.HasIndex(h => h.Name).IsUnique();

        var rooms = modelBuilder.Entity<Room>();
        rooms.HasKey(r => r.RoomId);
        rooms.Property(r => r.RoomId).ValueGeneratedOnAdd().UseIdentityColumn(100000);
        rooms
            .HasMany(r => r.Bookings)
            .WithOne(r => r.Room)
            .HasForeignKey(r => r.RoomId);
        rooms.HasIndex(r => new { r.HotelId, r.Name }).IsUnique();
        rooms.Property(r => r.Name).HasMaxLength(100);
        rooms.Property(r => r.Type).HasMaxLength(100);

        // The index on the RoomId is meant to support the logic for confirming a booking
        // attempt in a situation where another booking has recently been made for the same room.

        var bookings = modelBuilder.Entity<Booking>();
        bookings.HasKey(b => b.BookingId);
        bookings.Property(b => b.BookingId).HasMaxLength(100);
        bookings.HasIndex(b => b.RoomId);
        bookings.Property(b => b.MinDate).HasConversion<DateOnly>();
        bookings.Property(b => b.MaxDate).HasConversion<DateOnly>();
        bookings.Property(b => b.UserId).HasMaxLength(100);
    }
}