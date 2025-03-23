using Microsoft.Extensions.DependencyInjection;
using TicketBooking.Services;


var serviceProvider = new ServiceCollection()
            .AddSingleton<IMovieTheaterService, MovieTheaterService>()
            .AddSingleton<IBookingService, BookingService>()
            .BuildServiceProvider();

// Resolve the service and use it
var bookingService = serviceProvider.GetService<IBookingService>();
//serviceC.DoWorkC();
