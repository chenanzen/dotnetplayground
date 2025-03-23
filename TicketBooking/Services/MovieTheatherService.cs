using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Services
{
    internal interface IMovieTheaterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numOfRow"></param>
        /// <param name="numOfSeatPerRow"></param>
        void Reset(int numOfRow, int numOfSeatPerRow);

        List<List<Seat>> GetSeats();
        List<Seat> GetRows(int rowNo);
        void BookSeat(string bookingNo, int row, int col);
        void ConfirmSeat();
    }

    internal class MovieTheaterService : IMovieTheaterService
    {
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<List<Seat>> Seats { get; set; } = new List<List<Seat>>();
        public MovieTheaterService() { }

        public void Reset(int numOfRow, int numOfSeatPerRow)
        {
            Bookings = new List<Booking>();
            Seats = new List<List<Seat>>();
            for (int row = 0; row < numOfRow; row++)
            {
                var newrow = new List<Seat>();
                for (int col = 0; col < numOfSeatPerRow; col++) newrow.Add(new Seat());
                Seats.Add(newrow);
            }

        }

        public List<List<Seat>> GetSeats()
        {
            throw new NotImplementedException();
        }

        public List<Seat> GetRows(int rowNo)
        {
            if (rowNo < 0 || rowNo >= Seats.Count) return new List<Seat>();
            else return Seats[0];
        }

        public void BookSeat(string bookingNo, int row, int col)
        {
            try
            {
                Seats[row][col].BookingNumber = bookingNo;
                Seats[row][col].Status = SeatBookingStatus.Booking;
            }
            catch (Exception )
            {
                // log and rethrow
                throw;
            }
        }

        public void ConfirmSeat()
        {
            foreach (var row in Seats)
            {
                foreach(var seat in row)
                {
                    if (seat.Status == SeatBookingStatus.Booking)
                        seat.Status = SeatBookingStatus.Booked;
                }
            }
        }
    }
}
