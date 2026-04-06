namespace HotelBooking.Responses;

/// <summary>
/// The result of a query to retrieve booking details.
/// </summary>
/// <param name="BookingId">The unique identifier for the booking.</param>
/// <param name="HotelName">The name of the hotel.</param>
/// <param name="Room">The room number (could potentially include letters too).</param>
/// <param name="RoomType">The room type.</param>
/// <param name="UserId">The ID of the user who made the booking.</param>
/// <param name="CheckIn">The arrival date.</param>
/// <param name="CheckOut">The departure date.</param>
public record BookingResponse(
    string BookingId,
    string HotelName,
    string Room,
    string RoomType,
    string UserId,
    DateOnly CheckIn,
    DateOnly CheckOut
);