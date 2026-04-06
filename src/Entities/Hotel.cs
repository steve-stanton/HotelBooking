namespace HotelBooking.Entities;

/// <summary>
/// Details for a hotel.
/// </summary>
public class Hotel
{
    /// <summary>
    /// The unique identifier of the hotel.
    /// </summary>
    public int HotelId { get; set; }

    /// <summary>
    /// The user-perceived name of the hotel.
    /// </summary>
    public string Name { get; set; } = "";
}