using HotelBooking.Entities;
using HotelBooking.Requests;
using HotelBooking.Responses;
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

    /// <inheritdoc cref="IBookingService.FindRooms"/>
    public async Task<FindRoomsResponse> FindRooms(FindRoomsRequest request, CancellationToken cancellation)
    {
        // TODO: Double-check for dates that might be 1-off
        
        var rooms = await (
            from r in _context.Rooms
            where r.HotelId == request.HotelId
                  && r.Capacity >= request.GuestCount
                  && !r.Bookings.Any(b =>
                      b.MinDate < request.Checkout &&
                      b.MaxDate > request.Checkin)
            select r
        ).ToListAsync(cancellation);

        return new FindRoomsResponse(
            request,
            rooms.Select(r =>
                new AvailableRoom(r.RoomId, r.BookingCount)).ToArray()
            );
    }

    /// <inheritdoc cref="IBookingService.BookRoom"/>
    /// <exception cref="ArgumentException">Thrown if the request contains details that don't match the room details.</exception>'
    public async Task<string?> BookRoom(BookRoomRequest request, string userId, CancellationToken cancellation)
    {
        var room = await _context.Rooms.FindAsync([request.Room.RoomId], cancellation);
        if (room is null)
            throw new ArgumentException("Unexpected room ID");
        
        if (room.HotelId != request.Requirement.HotelId)
            throw new ArgumentException("Unexpected hotel ID");
        
        if (room.Capacity < request.Requirement.GuestCount)
            throw new ArgumentException("Room capacity is not large enough");
        
        // Check to see if someone else has recently booked the room. If so, increment the conflict count
        // and return with an undefined booking ID.
        if (room.BookingCount != request.Room.BookingCount)
        {
            room.ConflictCount++;
            await _context.SaveChangesAsync(cancellation);
            return null;
        }
        
        // Make the booking. If database access is heavily concurrent, it's conceivable that someone else
        // could be making a simultaneous booking for the same room. In that case, the database should
        // end up with a primary key conflict.
        var bookingId = $"{request.Room.RoomId}.{request.Room.BookingCount + 1}";
        room.BookingCount++;
        _context.Bookings.Add(new Booking
        {
            BookingId = bookingId,
            RoomId = room.RoomId,
            HotelId = room.HotelId,
            MinDate = request.Requirement.Checkin,
            MaxDate = request.Requirement.Checkout.AddDays(-1),
            UserId = userId
        });
        await _context.SaveChangesAsync(cancellation);
        return bookingId;
    }
}