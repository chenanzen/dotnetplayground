using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking
{
    interface IMovieTheaterServivce
    {

    }

    internal class MovieTheaterService : IMovieTheaterServivce
    {
        public List<List<T>> Seats { get; set; }
        public MovieTheaterService() { }
    }
}
