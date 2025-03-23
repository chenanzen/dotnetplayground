using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Services
{
    public static class TBUtil
    {
        public static char NumberToLetter(int number)
        {
            if (number < 0 || number > 25)
                throw new ArgumentOutOfRangeException(nameof(number), "Number must be between 0 and 25");

            return (char)('A' + number);
        }

        public static int LetterToNumber(char letter)
        {
            letter = char.ToUpper(letter);
            var result = 0;
            if (letter >= 'A' && letter <= 'Z')
            {
                // log: "Letter must be a capital letter between A and Z"
                result = letter - 'A';
            }

            return result;
        }
    }
}
