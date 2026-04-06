namespace HotelBooking;

/// <summary>
/// The identity of a user.
/// </summary>
public interface IUserPrincipal
{
    /// <summary>
    /// The unique identifier for a user.
    /// </summary>
    string UserId { get; }
}