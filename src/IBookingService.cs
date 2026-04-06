using HotelBooking.Entities;
using HotelBooking.Requests;

namespace HotelBooking;

public interface IBookingService
{
    /// <summary>
    /// Seeds the database with a set of hotels and rooms.
    /// </summary>
    /// <param name="request">Details for items to insert into the database.</param>
    /// <remarks>
    /// This is included here to assist with testing. It is not intended for use in production.
    /// </remarks>
    Task SeedDatabase(SeedDatabaseRequest request);
    
    /// <summary>
    /// Retrieves hotel details, given the name of the hotel.
    /// </summary>
    /// <param name="hotelName">The name of the hotel to retrieve (case-sensitive, no wildcards).</param>
    /// <returns>The hotel details (null if not found).</returns>
    Task<HotelDetail?> GetHotelByName(string hotelName);
}