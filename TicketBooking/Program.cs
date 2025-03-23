using Microsoft.Extensions.DependencyInjection;
using TicketBooking.Services;
using static TicketBooking.Services.TBUtil;


var serviceProvider = new ServiceCollection()
            .AddSingleton<IMovieTheaterService, MovieTheaterService>()
            .AddSingleton<IBookingService, BookingService>()
            .AddSingleton<IInputParserService, InputParserService>()
            .BuildServiceProvider();

// Resolve the service and use it
var bookingService = serviceProvider.GetService<IBookingService>();
var inputParserService = serviceProvider.GetService<IInputParserService>();
var movieTheaterService = serviceProvider.GetService<IMovieTheaterService>();
if (inputParserService == null || movieTheaterService == null || bookingService == null)
{
    Console.WriteLine("Unexpected error(s) occured when initializing service");
    return;
}

string? input;
bool goodInputEntered = false;

// get theater detail
TheaterDetail theaterDetail = null;
while (theaterDetail == null)
{
    Console.WriteLine("Please define movie title and seating map in [Title] [Row] [Seat PerRow] format:");
    Console.Write("> ");
    input = Console.ReadLine();
    
    // exit when empty space entered
    if (string.IsNullOrEmpty(input)) return;

    theaterDetail = inputParserService.TryParseTheaterInput(input);
}

// show menu
if (theaterDetail != null)
{
    movieTheaterService.Reset(theaterDetail.Row, theaterDetail.SeatPerRow);

    MenuSelection menuSelection = MenuSelection.Invalid;
    while (menuSelection == MenuSelection.Invalid)
    {
        var noOfAvailableSeats = movieTheaterService.GetNoOfAvailableSeats();

        Console.WriteLine();
        Console.WriteLine("Welcome to GIC Cinemas");
        Console.WriteLine($"[1] Book tickets for {theaterDetail.Title} ({noOfAvailableSeats} seats available)");
        Console.WriteLine("[2] Check bookings");
        Console.WriteLine("[3] Exit");
        Console.WriteLine("Please enter your selection:");
        Console.Write("> ");
        input = Console.ReadLine();

        menuSelection = inputParserService.TryParseMenuInput(input);
        switch (menuSelection)
        {
            case MenuSelection.Invalid: 
                break;
            case MenuSelection.Book: 
                break;
            case MenuSelection.Check: 
                break;
            case MenuSelection.Exit: 
                // do nothing, it will exit
                break;
        }
    }


}