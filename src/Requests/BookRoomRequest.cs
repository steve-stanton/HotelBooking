using HotelBooking.Responses;

namespace HotelBooking.Requests;

/// <summary>
/// Request to book a room.
/// </summary>
/// <param name="Requirement">The query parameters that were used to find available rooms.</param>
/// <param name="Room">The room to book (one of the rooms found via a previous availability check).</param>
/// <seealso cref="BookRoomResponse"/>
/// <example>
/// {
///   "requirement": {
///     "hotelId": 1000,
///     "guestCount": 1,
///     "checkin": "2026-08-31",
///     "checkout": "2026-09-02"
///   },
///   "room": {
///     "roomId": 100004,
///     "bookingCount": 0
///   }
/// }
/// </example>
public record BookRoomRequest(FindRoomsRequest Requirement, AvailableRoom Room);