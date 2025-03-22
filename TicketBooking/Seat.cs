using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking
{
    internal class Seat
    {
        public string BookingNumber { get; set; }
        public SeatBookingStatus Status { get; set; }

    }

    enum SeatBookingStatus
    {
        Avail,
        Booking,
        Booked
    }
}
