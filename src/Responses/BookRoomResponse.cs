using HotelBooking.Requests;

namespace HotelBooking.Responses;

/// <summary>
/// The response for a <see cref="BookRoomRequest"/>.
/// </summary>
/// <param name="BookingId">The ID that was assigned for the booking.</param>
public record BookRoomResponse(string BookingId);