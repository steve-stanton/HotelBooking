using HotelBooking.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers;

[ApiController]
[Route("booking")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    
    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
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
}