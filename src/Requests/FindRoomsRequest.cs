namespace HotelBooking.Requests;

/// <summary>
/// Request to find available rooms in a hotel.
/// </summary>
/// <param name="HotelId">The ID of the hotel.</param>
/// <param name="GuestCount">The number of guests.</param>
/// <param name="Checkin">The date when the guests will arrive.</param>
/// <param name="Checkout">The date when the guests will leave (a date greater than the checkin date).</param>
public record FindRoomsRequest
    ( int HotelId
    , int GuestCount
    , DateOnly Checkin
    , DateOnly Checkout);