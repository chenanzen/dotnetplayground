using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TicketBooking.Services;
using static TicketBooking.Services.TBUtil;


var serviceProvider = new ServiceCollection()
            .AddSingleton<IMovieTheaterService, MovieTheaterService>()
            .AddSingleton<IBookingService, BookingService>()
            .AddSingleton<IInputParserService, InputParserService>()
            .AddSingleton<IInputService, InputService>()
            .BuildServiceProvider();

// Resolve the service and use it
var bookingService = serviceProvider.GetService<IBookingService>();
var inputParserService = serviceProvider.GetService<IInputParserService>();
var movieTheaterService = serviceProvider.GetService<IMovieTheaterService>();
var inputService = serviceProvider.GetService<IInputService>();

if (inputParserService == null || movieTheaterService == null || bookingService == null || inputService == null)
{
    Console.WriteLine("Unexpected error(s) occured when initializing service");
    return;
}

// get theater detail, keep asking until quit or valid input
var theaterDetail = inputService.GetTheaterDetail();

// show menu
if (theaterDetail != null)
{
    movieTheaterService.Reset(theaterDetail.Title, theaterDetail.Row, theaterDetail.SeatPerRow);

    var menuSelection = MenuSelection.Invalid;
    while(menuSelection != MenuSelection.Exit)
    {
        menuSelection = inputService.GetMenuSelection();
        switch (menuSelection)
        {
            case MenuSelection.Invalid:
                break;
            case MenuSelection.Book:
                inputService.MakeBookings();
                break;
            case MenuSelection.Check:
                inputService.CheckBookings();
                break;
            case MenuSelection.Exit:
                inputService.Exit();
                break;
        }
    }
}

