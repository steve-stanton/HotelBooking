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
}