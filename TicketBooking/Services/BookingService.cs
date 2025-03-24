using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Services
{
    interface IBookingService
    {
        /// <summary>
        /// Find if there are enough seats available for booking
        /// </summary>
        /// <param name="numOfTicket">number of ticket requested</param>
        /// <returns>true if there are enough ticket; false other wise</returns>
        bool IsAvailable(int numOfTicket);

        string Book(int numOfTicket);

        string ChangeSeat(string bookingNo, string preferredSeat);

        void Confirm();

        Point TranslateToPoint(string preferredSeat);

        string GenerateBookingNumber();

        int ReserveRow(string bookingNo, int numOfTicket, int preferredSeatNo, int rowNo);
    }

    internal class BookingService : IBookingService
    {
        private readonly IMovieTheaterService _movieTheaterService;

        public BookingService(IMovieTheaterService movieTheaterServivce)
        {
            _movieTheaterService = movieTheaterServivce;
        }

        public string GenerateBookingNumber()
        {
            var totalNumOfBooking = _movieTheaterService.GetSeats()
                .SelectMany(r => r.Where(s => s.Status != SeatBookingStatus.Avail)
                                  .Select(s => s.BookingNumber))
                .Distinct().Count();
            var bookingIdx = totalNumOfBooking + 1;
            var bookingNo = $"GIC{bookingIdx:D4}";

            return bookingNo;
        }

        public Point TranslateToPoint(string preferredSeat)
        {
            var coordinateToTranslate = string.IsNullOrWhiteSpace(preferredSeat) ? "A0" : preferredSeat;
            int rowNo = 0;
            rowNo = TBUtil.LetterToNumber(coordinateToTranslate.First());
            int seatNo;
            if (!int.TryParse(coordinateToTranslate.Substring(1), out seatNo)) seatNo = 0;

            // cannot be lower than zero
            if (seatNo < 0) seatNo = 0;
            // cannot be higher than total seat no

            seatNo = seatNo - 1;
            return new Point(rowNo, seatNo);
        }

        public bool IsAvailable(int numOfTicket)
        {
            var rows = _movieTheaterService.GetSeats();
            var numOfAvailSeats = rows.SelectMany(r => r.Select(s => s)).Count(s => s.Status == SeatBookingStatus.Avail);
            if (numOfAvailSeats > numOfTicket)
            {
                return true;
            }

            return false;
        }

        public int ReserveRow(string bookingNo, int numOfTicket, int preferredSeatNo, int rowNo)
        {
            int numOfTicketReserved = 0;

            var row =_movieTheaterService.GetRows(rowNo);

            int assignedSeat = preferredSeatNo;
            // if no preferredSeatNo, set to default logic of find closest to mid
            if (preferredSeatNo == -1)
            {
                if (row.Count > numOfTicket)
                    assignedSeat = (row.Count - numOfTicket) / 2;
                else assignedSeat = 0;

                // ensure assigned seat is available, if not available go to seat to right
                for (int i = assignedSeat; i < row.Count; i++) 
                {
                    if (row[i].Status == SeatBookingStatus.Avail)
                    {
                        assignedSeat = i;
                        break;
                    }
                }
            }

            // start chopping seat
            for (int i = assignedSeat; (i < row.Count && numOfTicketReserved < numOfTicket); i++)
            {
                if (row[i].Status == SeatBookingStatus.Avail)
                {
                    _movieTheaterService.BookSeat(bookingNo, rowNo, i);
                    numOfTicketReserved++;
                }
            }

            return numOfTicketReserved;
        }

        public string Book(int numOfTicket)
        {
            var rows = _movieTheaterService.GetSeats();

            // preference
            var preferredSeatPoint = TranslateToPoint("");

            // generate booking number
            var bookingNo = GenerateBookingNumber();

            var ticketLeftToReserved = numOfTicket;
            var preferredRow = preferredSeatPoint.X;
            var preferredSeatNo = preferredSeatPoint.Y;
            while (ticketLeftToReserved > 0)
            {
                var reservedTickets = ReserveRow(bookingNo, ticketLeftToReserved, preferredSeatNo, preferredRow);

                // go to next row
                preferredRow++;
                preferredSeatNo = -1;

                // check if we still need further ticket to 
                ticketLeftToReserved = ticketLeftToReserved - reservedTickets;
            }

            return bookingNo;
        }

        public string ChangeSeat(string bookingNo, string preferredSeat)
        {
            var rows = _movieTheaterService.GetSeats();

            // translate entered preferred seat into x, y points
            var preferredSeatPoint = TranslateToPoint(preferredSeat);

            // if user entered preferred row is larger than theater size, reset to nothing selected
            if (preferredSeatPoint.X >= rows.Count)
                preferredSeatPoint.X = 0;

            // remove current reservation
            var reserved = rows.SelectMany(r => r.Select(s => s)).Where(s => s.BookingNumber == bookingNo).ToList();
            var numOfTicket = reserved.Count;
            foreach (var item in reserved)
            {
                item.BookingNumber = "";
                item.Status = SeatBookingStatus.Avail;
            }

            // reserve with seats preferences
            var ticketLeftToReserved = numOfTicket;
            var preferredRow = preferredSeatPoint.X;
            var preferredSeatNo = preferredSeatPoint.Y;
            while (ticketLeftToReserved > 0)
            {
                var reservedTickets = ReserveRow(bookingNo, ticketLeftToReserved, preferredSeatNo, preferredRow);

                // go to next row
                preferredRow++;
                preferredSeatNo = -1;

                // check if we still need further ticket to 
                ticketLeftToReserved = ticketLeftToReserved - reservedTickets;
            }

            return bookingNo;

        }

        public void Confirm()
        {
            _movieTheaterService.ConfirmSeat();
        }
    }
}
