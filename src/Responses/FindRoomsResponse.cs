using HotelBooking.Requests;

namespace HotelBooking.Responses;

/// <summary>
/// The result of a query to find available rooms in a hotel.
/// </summary>
/// <param name="Request">The query parameters.</param>
/// <param name="Rooms">The rooms that are available.</param>
public record FindRoomsResponse(
    FindRoomsRequest Request,
    AvailableRoom[] Rooms);
    
/// <summary>
/// Detail for a room that may be booked.
/// </summary>
/// <param name="RoomId">The ID of a room that is available.</param>
/// <param name="BookingCount">The number of bookings previously made for the room during its lifetime.</param>
public record AvailableRoom(
    int RoomId,
    int BookingCount);    