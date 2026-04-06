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
    [HttpPost("admin/seed-database")]
    public async Task<ActionResult> SeedDatabase(SeedDatabaseRequest databaseRequest)
    {
        await _bookingService.SeedDatabase(databaseRequest);
        return Created();
    }
}