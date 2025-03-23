using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TicketBooking.Services.TBUtil;

namespace TicketBooking.Services
{
    interface IInputParserService
    {
        TheaterDetail TryParseTheaterInput(string input);

        MenuSelection TryParseMenuInput(string? input);
    }

    internal class InputParserService : IInputParserService
    {
        private readonly char[] _seperator = { ' ' };

        public TheaterDetail TryParseTheaterInput(string input)
        {
            TheaterDetail output = null;

            var tokens = input.Split(_seperator);
            if (tokens.Any())
            {
                if (tokens.Length == 3)
                {
                    var title = tokens[0] ?? "";
                    var titleOK = !string.IsNullOrWhiteSpace(title);
                    var rowOK = Int32.TryParse(tokens[1] ?? "", out int row);
                    var colOK = Int32.TryParse(tokens[2] ?? "", out int col);
                    if (titleOK && rowOK && colOK)
                    {
                        output = new TheaterDetail()
                        {
                            Title = title,
                            Row = row,
                            SeatPerRow = col
                        };

                        return output;
                    }
                }
            }

            return output;
        }

        public MenuSelection TryParseMenuInput(string? input)
        {
            MenuSelection menuSelection;
            if (!Enum.TryParse<MenuSelection>(input, out menuSelection))
                menuSelection = MenuSelection.Invalid;

            return menuSelection;
        }

    }
}
