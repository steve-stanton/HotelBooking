namespace HotelBooking;

/// <summary>
/// Represents a user who can make an anonymous booking.
/// </summary>
/// <param name="UserId">The fixed ID for all anonymous users.</param>
/// <remarks>
/// The ability to make anonymous bookings may be useful during active development,
/// and when setting up tests. In a production setting, the expectation is that all
/// bookings will be made by authenticated end-users.
/// </remarks>
public record AnonymousUser(string UserId = "anon") : IUserPrincipal;
