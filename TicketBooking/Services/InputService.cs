using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Services
{
    interface IInputService
    {
        void MakeBookings();
        void CheckBookings();
    }

    internal class InputService : IInputService
    {
        private readonly IMovieTheaterService _movieTheaterService;
        private readonly IBookingService _bookingService;

        public InputService(IMovieTheaterService movieTheaterService, IBookingService bookingService) 
        {
            _movieTheaterService = movieTheaterService;
            _bookingService = bookingService;
        }

        public void MakeBookings()
        {
            bool keepLooping = true;
            while (keepLooping)
            {
                Console.WriteLine();
                Console.WriteLine("Enter number of tickets to book, or enter blank to go back to main menu: ");
                var numOfTicketToBookString = getInput();

                if (string.IsNullOrEmpty(numOfTicketToBookString))
                {
                    keepLooping = false;
                }
                else
                {
                    bool canParse = Int32.TryParse(numOfTicketToBookString, out int numOfTicketToBook);
                    if (canParse)
                    {
                        var availSeats = _movieTheaterService.GetNoOfAvailableSeats();
                        if (numOfTicketToBook > availSeats)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Sorry, there are only {availSeats} seats available.");
                        }
                        else
                        {
                            var bookingNo = _bookingService.Book(numOfTicketToBook);
                            if (string.IsNullOrEmpty(bookingNo))
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Sorry, unable to make booking due to unforseen circumtances. Please contact tech support at itsupport@gic.com.sg.");
                                keepLooping = false;
                            }
                            else
                            {
                                var movieTitle = _movieTheaterService.GetMovieTitle();

                                var seats = _movieTheaterService.GetSeats();

                                Console.WriteLine($"Successfully reserved {numOfTicketToBook} {movieTitle} tickets.");
                                Console.WriteLine($"Booking Id: {bookingNo}");
                                Console.WriteLine($"Selected seats:");
                                printMap(bookingNo, seats);

                                Console.WriteLine();
                                Console.WriteLine("Enter blank to accept seat selection, or enter new seating position:");
                                var preferredSeat = getInput();
                                if (string.IsNullOrEmpty(preferredSeat))
                                {
                                    // confirm
                                    _bookingService.Confirm(bookingNo);
                                    Console.WriteLine($"Booking Id: {bookingNo} confirmed.");
                                }
                                else
                                {
                                    // change seat
                                    bookingNo = _bookingService.ChangeSeat(bookingNo, preferredSeat);
                                    Console.WriteLine();
                                    Console.WriteLine($"Booking Id: {bookingNo}");
                                    Console.WriteLine($"Selected seats:");
                                    Console.WriteLine();
                                    printMap(bookingNo, seats);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void printMap(string bookingNo, List<List<Seat>> seats)
        {
            var seatsPerRow = seats[0].Count;
            var width = seatsPerRow * 2;
            var stringToPrint = "S C R E E N";
            var numOfSpacePrefix = (width - stringToPrint.Length) / 2;
            var numOfSpaceSuffix = (width - stringToPrint.Length - numOfSpacePrefix);
            Console.WriteLine($"{new string(' ', numOfSpacePrefix)}{stringToPrint}{new string(' ', numOfSpaceSuffix)}");
            Console.WriteLine($"{new string('-', width)}");
            for (int i = seats.Count - 1; i >= 0; i--)
            {
                Console.Write($"{TBUtil.NumberToLetter(i)} ");
                for (int j = 0; j < seatsPerRow; j++)
                {
                    var charToPrint = ".";
                    if (seats[i][j].Status == SeatBookingStatus.Booking) charToPrint = "o";
                    else if (seats[i][j].Status == SeatBookingStatus.Booked) charToPrint = "#";
                    Console.Write($"{charToPrint} ");
                }
                Console.WriteLine();
            }
        }

        public void CheckBookings()
        {
            Console.WriteLine();
            Console.WriteLine("Enter booking id, or enter blank to go back to main menu:");

            var bookingIdTofind = getInput();

            _movieTheaterService.GetSeats();
        }

        private string getInput()
        {
            Console.Write("> ");
            var input = Console.ReadLine() ?? "";
            return input;
        }
    }
}
