namespace HotelBooking.Requests;

/// <summary>
/// Request to seed the database with a set of hotels and rooms.
/// </summary>
/// <param name="Hotels">Details for the hotels to be inserted.</param>
/// <example>
/// {
///   "hotels": [
///    {
///      "hotelName": "A",
///      "singleRoomCount": 6
///     },
///    {
///      "hotelName": "B",
///      "doubleRoomCount": 6
///     },
///    {
///      "hotelName": "C",
///      "singleRoomCount": 2,
///      "doubleRoomCount": 2,
///      "deluxeRoomCount": 2
///     }
///   ]
/// }
/// </example>
public record SeedDatabaseRequest(HotelDetail[] Hotels);

/// <summary>
/// Summary details for a hotel.
/// </summary>
/// <param name="HotelName">The name of the hotel</param>
/// <param name="SingleRoomCount">The number of single rooms in the hotel.</param>
/// <param name="DoubleRoomCount">The number of double rooms in the hotel.</param>
/// <param name="DeluxeRoomCount">The number of deluxe rooms in the hotel.</param>
public record HotelDetail
    ( string HotelName
    , int SingleRoomCount = 0
    , int DoubleRoomCount = 0
    , int DeluxeRoomCount = 0);
