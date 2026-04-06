namespace HotelBooking.Entities;

/// <summary>
/// A room within a <see cref="Hotel"/>.
/// </summary>
public class Room
{
    /// <summary>
    /// The unique identifier of the room.
    /// </summary>
    public int RoomId { get; set; }
    
    /// <summary>
    /// The unique identifier of the <see cref="Hotel"/> that the room is in.
    /// </summary>
    public int HotelId { get; set; }

    /// <summary>
    /// Navigation property to get the associated hotel.
    /// </summary>
    public Hotel Hotel { get; set; } = null!;
    
    /// <summary>
    /// The name of the room (typically the room number).
    /// All room names for a given hotel must be unique.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Text describing the room type (eg. Single, Double, Deluxe). 
    /// </summary>
    public string Type { get; set; } = "";
    
    /// <summary>
    /// The maximum number of guests that can occupy the room (expected to be greater than 0).
    /// </summary>
    public int Capacity { get; set; }
    
    /// <summary>
    /// The number of bookings that have been successfully made for the room during its lifetime.
    /// </summary>
    public int BookingCount { get; set; }
    
    /// <summary>
    /// The number of times that a booking was attempted, but which could not be completed
    /// because another booking had been made.
    /// </summary>
    /// <remarks>
    /// Before making a booking for a range of dates, a user must initially retrieve details for the rooms that are
    /// available for the duration. To actually book a room, the request needs to include the <see cref="RoomId"/>,
    /// as well as the value of <see cref="BookingCount"/> that the room had when the available rooms were retrieved.
    /// A booking succeeds so long as the room's booking count remains the same. That being the case, the booking count
    /// will be increased by 1.
    /// <para/>
    /// However, if someone else has booked the room in the meantime, the booking will fail (even if the other booking
    /// is for a completely different set of dates). In that scenario, the conflict count will be increased
    /// instead. The assumption is that conflicts will arise only very occasionally. Keeping track of the
    /// number of conflicts aims to prove whether this is actually the case.
    /// <para/>
    /// Following a conflict, the user will need to make a further request to retrieve the latest room availability.
    /// </remarks>
    public int ConflictCount { get; set; }

    /// <summary>
    /// Navigation property to access the bookings for the room.
    /// </summary>
    public List<Booking> Bookings { get; set; } = new();
}
