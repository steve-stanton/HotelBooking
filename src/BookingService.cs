using HotelBooking.Entities;
using HotelBooking.Requests;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking;

/// <summary>
/// Implementation of <see cref="IBookingService"/>.
/// </summary>
public class BookingService : IBookingService
{
    private readonly BookingContext _context;

    public BookingService(BookingContext context)
    {
        _context = context;
    }

    /// <inheritdoc cref="IBookingService.SeedDatabase"/>
    public async Task SeedDatabase(SeedDatabaseRequest request)
    {
        _context.Hotels.AddRange(request.Hotels.Select(h =>
        {
            var rooms = new List<Room>();
            rooms.AddRange(CreateRooms(rooms.Count + 1, h.SingleRoomCount, "Single", 1));
            rooms.AddRange(CreateRooms(rooms.Count + 1, h.DoubleRoomCount, "Double", 2));
            rooms.AddRange(CreateRooms(rooms.Count + 1, h.DeluxeRoomCount, "Deluxe", 4));
            
            return new Hotel
            {
                Name = h.HotelName,
                Rooms = rooms
            };
        }));

        await _context.SaveChangesAsync();
    }

    private IEnumerable<Room> CreateRooms(int firstRoomNumber, int roomCount, string type, int capacity)
    {
        foreach (var roomNumber in Enumerable.Range(firstRoomNumber, roomCount))
        {
            yield return new Room
            {
                Name = roomNumber.ToString(),
                Type = type,
                Capacity = capacity
            };
        }
    }

    /// <inheritdoc cref="IBookingService.GetHotelByName"/>
    public async Task<HotelDetail?> GetHotelByName(string hotelName)
    {
        var hotel = await _context.Hotels
            .Include(h => h.Rooms)
            .SingleOrDefaultAsync(h => h.Name == hotelName);

        if (hotel is null)
            return null;
        
        return new HotelDetail(
            hotel.Name,
            hotel.Rooms.Count(r => r.Type == "Single"),
            hotel.Rooms.Count(r => r.Type == "Double"),
            hotel.Rooms.Count(r => r.Type == "Deluxe"));
    }
}