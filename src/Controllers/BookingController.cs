using HotelBooking.Requests;
using HotelBooking.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("booking")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IUserPrincipal _userPrincipal;
    private readonly TimeProvider _timeProvider;

    public BookingController(
        IBookingService bookingService,
        IUserPrincipal userPrincipal,
        TimeProvider timeProvider)
    {
        _bookingService = bookingService;
        _userPrincipal = userPrincipal;
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Seeds the database with a set of hotels and rooms (without any bookings).
    /// </summary>
    [HttpPost("admin/seed-database", Name = nameof(SeedDatabase))]
    public async Task<ActionResult> SeedDatabase(SeedDatabaseRequest databaseRequest)
    {
        await _bookingService.SeedDatabase(databaseRequest);
        return Created();
    }

    /// <summary>
    /// Retrieves hotel details, given the name of the hotel. 
    /// </summary>
    /// <param name="hotelName">The name of the hotel to retrieve (case-sensitive, no wildcards).</param>
    /// <response code="200">The hotel details.</response>
    /// <response code="404">A hotel with the specified name was not found.</response>
    [HttpGet("hotel-by-name", Name = nameof(GetHotelByName))]
    [ProducesResponseType(typeof(HotelDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HotelDetail?>> GetHotelByName(string hotelName)
    {
        var hotel = await _bookingService.GetHotelByName(hotelName);

        if (hotel is null)
            return NotFound();

        return Ok(hotel);
    }

    /// <summary>
    /// Attempts to find rooms that are available for booking.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel to search.</param>
    /// <param name="guestCount">The number of guests who will be occupying the room.</param>
    /// <param name="checkinDate">The date when the guests will arrive (a date greater than
    /// or equal to the current date).</param>
    /// <param name="checkoutDate">The date when the guests will leave (a date greater than
    /// the checkin date).</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the request.</param>
    /// <returns>A list of the suitable rooms (if any) that are available in the requested
    /// date range.</returns>
    [HttpGet("find-rooms", Name = nameof(FindRooms))]
    [ProducesResponseType(typeof(FindRoomsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FindRoomsResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FindRoomsResponse>> FindRooms(
        int hotelId,
        int guestCount,
        DateOnly checkinDate,
        DateOnly checkoutDate,
        CancellationToken cancellation)
    {
        // TODO: Handle timezone offset between hotel and api server
        var today = DateOnly.FromDateTime(_timeProvider.GetLocalNow().Date);
        if (checkinDate < today)
        {
            return Problem(
                title: "Invalid request", 
                detail: "Checkin date must be greater than or equal to today's date.", 
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (checkoutDate <= checkinDate)
        {
            return Problem(
                title: "Invalid request", 
                detail: "Checkin date must be greater than or equal to today's date.", 
                statusCode: StatusCodes.Status400BadRequest);
        }
        
        var request = new FindRoomsRequest(hotelId, guestCount, checkinDate, checkoutDate);
        var response = await _bookingService.FindRooms(request, cancellation);

        return Ok(response);
    }

    /// <summary>
    /// Attempts to book a room.
    /// </summary>
    /// <param name="request">The booking request parameters.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the request.</param>
    /// <returns>The booking confirmation.</returns>
    [HttpPost(Name = nameof(BookRoom))]
    [ProducesResponseType(typeof(BookRoomResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookRoomResponse>> BookRoom(BookRoomRequest request, CancellationToken cancellation)
    {
        try
        {
            var bookingId = await _bookingService.BookRoom(request, _userPrincipal.UserId, cancellation);
        
            if (bookingId is null)
                return Conflict("Room is no longer available");

            return Created($"/booking/{bookingId}", new BookRoomResponse(bookingId));
        }
        catch (ArgumentException e)
        {
            return Problem(e.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }
}