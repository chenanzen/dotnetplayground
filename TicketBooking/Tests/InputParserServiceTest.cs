using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Services;
using Xunit;

namespace TicketBooking.Tests
{
    public class InputParserServiceTest
    {
        private readonly IInputParserService _inputParserService;

        public InputParserServiceTest()
        {
            _inputParserService = new InputParserService();
        }

        [Theory]
        [InlineData("Inception 8 10", true, "Inception", 8, 10)]
        [InlineData("Inception810", false, "", 0, 0)]
        public void TryParseTheaterInputTest(string input, bool expectedCanParse, string expectedTitle, int expectedRow, int expectedSeatPerRow)
        {
            TheaterDetail output = _inputParserService.TryParseTheaterInput(input);

            if (!expectedCanParse)
            {
                Assert.Null(output);
            }
            else
            {
                Assert.NotNull(output);
                Assert.Equal(expectedTitle, output.Title);
                Assert.Equal(expectedRow, output.Row);
                Assert.Equal(expectedSeatPerRow, output.SeatPerRow);
            }
        }
    }
}
