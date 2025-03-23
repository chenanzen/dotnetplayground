using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Services
{
    internal class Seat
    {
        public string BookingNumber { get; set; } = string.Empty;
        public SeatBookingStatus Status { get; set; } = SeatBookingStatus.Avail;

        public Seat()
        {
            BookingNumber = string.Empty;
            Status = SeatBookingStatus.Avail;
        }
    }

    enum SeatBookingStatus
    {
        Avail,
        Booking,
        Booked
    }
}
