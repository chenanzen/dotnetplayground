using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Services
{
    internal class Booking
    {
        public string BookingNo { get; set; } = string.Empty;
        public List<Seat> BookedSeats { get; set; } = new List<Seat>();

        public Booking(string bookingNo)
        {
            BookingNo = bookingNo;
            BookedSeats = new List<Seat>();
        }
    }
}
