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

public record HotelDetail
    ( string HotelName
    , int SingleRoomCount = 0
    , int DoubleRoomCount = 0
    , int DeluxeRoomCount = 0);
