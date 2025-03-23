using Xunit;
using System.Collections.Generic;
using System.Drawing;
using TicketBooking.Services;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TicketBooking.Tests
{
    public class BookingServiceTest
    {
        private readonly IMovieTheaterService _movieTheaterService;
        private readonly IBookingService _bookingService;

        public BookingServiceTest() 
        {
            _movieTheaterService = new MovieTheaterService();
            _bookingService = new BookingService(_movieTheaterService);

            _movieTheaterService.Reset(10, 10);
        }


        [Theory]
        [InlineData("", 0, -1)]
        [InlineData("B", 1, -1)]
        [InlineData("B     ", 1, -1)]
        [InlineData("C5", 2, 4)]
        [InlineData("c5", 2, 4)]
        [InlineData("D1", 3, 0)]
        [InlineData("D0", 3, -1)]
        [InlineData("D-1", 3, -1)]
        [InlineData("@-1", 0, -1)]
        public void TranslateToPointTest(string input, int expectedRowNo, int expectedSeatNo)
        {
            var result = _bookingService.TranslateToPoint(input);

            Assert.Equal(result.X, expectedRowNo);
            Assert.Equal(result.Y, expectedSeatNo);
        }

        [Theory]
        [InlineData(0, "GIC001")]
        [InlineData(2, "GIC003")]
        [InlineData(5, "GIC006")]
        public void GenerateBookingNumberTest(int numOfExistingBooking, string expectedBookingNumber)
        {
            // clear movie theater
            _movieTheaterService.Reset(10, 10);
            // create numOfExistingBooking fake entries
            for(int i=0; i<numOfExistingBooking; i++) _bookingService.Book(1);

            // generate booking number
            var generatedBookingNumber = _bookingService.GenerateBookingNumber();

            Assert.Equal(generatedBookingNumber, expectedBookingNumber);
        }

        [Theory]
        [InlineData(4, 0, -1, new int[] { }, new int[] { 3, 4, 5, 6 })]
        [InlineData(4, 0, 4, new int[] { 3, 4, 5, 6 }, new int[] { 7, 8, 9 })]
        public void ReserveRowTest(int numOfTicket, int rowNo, int preferredSeatNo
            , int[] filledSeats, int[] expectedReservedSeats)
        {
            // clear movie theater
            _movieTheaterService.Reset(10, 10);
            // init filledSeats
            var bookedNo = "booked";
            foreach (int filledSeat in filledSeats)
                _movieTheaterService.BookSeat(bookedNo, rowNo, filledSeat);
            _movieTheaterService.ConfirmSeat();

            var bookingNo = "test";
            var reservedNumOfTicket = _bookingService.ReserveRow(bookingNo, numOfTicket, preferredSeatNo, rowNo);

            var seats = _movieTheaterService.GetRows(rowNo);

            // 3, 4, 5, 6
            var reservedSeats = seats.Select((seat, idx) => new { seat, idx })
                .Where(x => x.seat.BookingNumber == bookingNo)
                .Select(x => x.idx);

            Assert.Equal(reservedNumOfTicket, expectedReservedSeats.Length);
            Assert.True(expectedReservedSeats.SequenceEqual(reservedSeats));
        }
    }
}