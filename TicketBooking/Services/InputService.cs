using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TicketBooking.Services.TBUtil;

namespace TicketBooking.Services
{
    interface IInputService
    {
        /// <summary>
        /// Get menu selection from user
        /// </summary>
        /// <returns>Menu selection of Book, Check or Exit</returns>
        MenuSelection GetMenuSelection();

        /// <summary>
        /// Get theater detail from user in format [Title] [Row] [Seat PerRow]
        /// enter blank to exit
        /// </summary>
        /// <returns>Theater detail to continue or null to exit</returns>
        TheaterDetail? GetTheaterDetail();

        void MakeBookings();
        void CheckBookings();
        void Exit();
    }

    internal class InputService : IInputService
    {
        private readonly IMovieTheaterService _movieTheaterService;
        private readonly IBookingService _bookingService;
        private readonly IInputParserService _inputParserService;
        public InputService(IMovieTheaterService movieTheaterService, IBookingService bookingService, 
            IInputParserService inputParserService) 
        {
            _movieTheaterService = movieTheaterService;
            _bookingService = bookingService;
            _inputParserService = inputParserService;
        }

        public TheaterDetail? GetTheaterDetail()
        {
            string? input;
            TheaterDetail? theaterDetail = null;
            while (theaterDetail == null)
            {
                Console.WriteLine("Please define movie title and seating map in [Title] [Row] [Seat PerRow] format:");
                input = getInput();
            
                // exit when empty space entered
                if (string.IsNullOrEmpty(input)) break;

                theaterDetail = _inputParserService.TryParseTheaterInput(input);
            }

            return theaterDetail;
        }

        public MenuSelection GetMenuSelection()
        {
            var title = _movieTheaterService.GetMovieTitle();
            var noOfAvailableSeats = _movieTheaterService.GetNoOfAvailableSeats();

            MenuSelection menuSelection = MenuSelection.Invalid;
            while (menuSelection == MenuSelection.Invalid)
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to GIC Cinemas");
                Console.WriteLine($"[1] Book tickets for {title} ({noOfAvailableSeats} seats available)");
                Console.WriteLine("[2] Check bookings");
                Console.WriteLine("[3] Exit");
                Console.WriteLine("Please enter your selection:");
                var input = getInput();

                menuSelection = _inputParserService.TryParseMenuInput(input);
            }

            return menuSelection;
        }

        public void MakeBookings()
        {
            var movieTitle = _movieTheaterService.GetMovieTitle();
            var noOfAvailableSeats = _movieTheaterService.GetNoOfAvailableSeats();
            var seats = _movieTheaterService.GetSeats();

            bool keepLooping = true;
            while (keepLooping)
            {
                keepLooping = true;

                Console.WriteLine();
                Console.WriteLine("Enter number of tickets to book, or enter blank to go back to main menu: ");
                var numOfTicketToBookString = getInput();

                bool canParse = Int32.TryParse(numOfTicketToBookString, out int numOfTicketToBook);
                if (!canParse)
                {
                    // invalid input, loop back to top to ask again
                    Console.WriteLine();
                    Console.WriteLine($"Sorry, I do not understand your input. Please enter number between 1 and {noOfAvailableSeats}.");
                    continue;
                }

                var availSeats = _movieTheaterService.GetNoOfAvailableSeats();
                if (numOfTicketToBook > availSeats)
                {
                    // not enough seats, loop back to top to ask again
                    Console.WriteLine();
                    Console.WriteLine($"Sorry, there are only {availSeats} seats available.");
                    continue;
                }

                var bookingNo = _bookingService.Book(numOfTicketToBook);
                if (string.IsNullOrEmpty(bookingNo))
                {
                    // exception occured, exit booking procedure
                    Console.WriteLine();
                    Console.WriteLine($"Sorry, unable to make booking due to unforseen circumtances. Please contact tech support at itsupport@gic.com.sg.");
                    break;
                }

                Console.WriteLine($"Successfully reserved {numOfTicketToBook} {movieTitle} tickets.");
                printMap(bookingNo, seats);

                var isConfirm = false;
                while (!isConfirm)
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter blank to accept seat selection, or enter new seating position:");
                    var preferredSeat = getInput();
                    if (string.IsNullOrEmpty(preferredSeat))
                    {
                        isConfirm = true;
                    }
                    else
                    {
                        // change seat
                        bookingNo = _bookingService.ChangeSeat(bookingNo, preferredSeat);
                        Console.WriteLine();
                        printMap(bookingNo, seats);
                    }
                }

                // confirm
                keepLooping = false;
                _bookingService.Confirm();
                Console.WriteLine();
                Console.WriteLine($"Booking Id: {bookingNo} confirmed.");
            }
        }

        public void printMap(string bookingNo, List<List<Seat>> seats)
        {
            var seatsPerRow = seats[0].Count;
            var width = seatsPerRow * 2;
            var stringToPrint = "S C R E E N";
            var numOfSpacePrefix = (width - stringToPrint.Length) / 2;
            var numOfSpaceSuffix = (width - stringToPrint.Length - numOfSpacePrefix);

            Console.WriteLine($"Booking Id: {bookingNo}");
            Console.WriteLine($"Selected seats:");
            Console.WriteLine();
            Console.WriteLine($"{new string(' ', numOfSpacePrefix)}{stringToPrint}{new string(' ', numOfSpaceSuffix)}");
            Console.WriteLine($"{new string('-', width)}");
            for (int i = seats.Count - 1; i >= 0; i--)
            {
                Console.Write($"{TBUtil.NumberToLetter(i)} ");
                for (int j = 0; j < seatsPerRow; j++)
                {
                    var charToPrint = ".";
                    if (seats[i][j].BookingNumber == bookingNo) charToPrint = "o";
                    else if (seats[i][j].BookingNumber != bookingNo && seats[i][j].Status != SeatBookingStatus.Avail) charToPrint = "#";
                    Console.Write($"{charToPrint} ");
                }
                Console.WriteLine();
            }
        }

        public void CheckBookings()
        {
            bool keepLooping = true;

            while (keepLooping)
            {
                Console.WriteLine();
                Console.WriteLine("Enter booking id, or enter blank to go back to main menu:");
                var bookingIdTofind = getInput();

                if(string.IsNullOrEmpty(bookingIdTofind))
                {
                    keepLooping = false;
                }
                else
                {
                    var seats = _movieTheaterService.GetSeats();
                    printMap(bookingIdTofind, seats);
                }
            }
        }

        public void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Thank you for using GIC Cinemas system. Bye!");
        }

        private string getInput()
        {
            Console.Write("> ");
            var input = Console.ReadLine() ?? "";
            return input;
        }

    }
}
