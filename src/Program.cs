using HotelBooking;
using HotelBooking.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

services
    .AddOpenApi() // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    .AddDbContext<BookingContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString);
    })
    .AddSingleton(TimeProvider.System)
    .AddScoped<IBookingService, BookingService>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        var docPath = Path.Combine(AppContext.BaseDirectory, "HotelBooking.xml");
        options.IncludeXmlComments(docPath);
    })
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "HotelBooking v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();