namespace HotelBooking.Entities;

/// <summary>
/// A booking made for a hotel room.
/// </summary>
public class Booking
{
    /// <summary>
    /// A unique identifier for the booking.
    /// </summary>
    /// <remarks>
    /// In the initial implementation, this will be a concatenation of the <see cref="RoomId"/> and the value
    /// of <see cref="Room.BookingCount"/> at the time of the booking. The aim is to ensure that the database
    /// can provide the last line of defence in a situation where we are dealing with a heavily concurrent
    /// environment, and duplicate bookings arise unexpectedly.
    /// </remarks>
    public string BookingId { get; set; } = "";
    
    /// <summary>
    /// The unique identifier of the <see cref="Room"/>.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Navigation property to get the associated room.
    /// </summary>
    public Room Room { get; set; } = null!;

    /// <summary>
    /// The ID of the <see cref="Hotel"/> that the room is in.
    /// </summary>
    /// <remarks>
    /// This is redundant as far as the database is concerned, given that the booking is associated with
    /// a room, and the room knows the hotel. It's included here as a convenience (e.g. would avoid a join
    /// on a lookup of bookings by hotel).
    /// </remarks>
    public int HotelId { get; set; }

    /// <summary>
    /// The checkin date (the first night staying in the room).
    /// </summary>
    public DateOnly MinDate { get; set; }
    
    /// <summary>
    /// The last night staying in the room (one day less than the checkout date).
    /// Must be greater than or equal to <see cref="MinDate"/>.
    /// </summary>
    public DateOnly MaxDate { get; set; }

    /// <summary>
    /// The unique identifier of the user who made the booking.
    /// </summary>
    public string UserId { get; set; } = "";
}