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

        //[Theory]
        //[ClassData(typeof(MyTestData))]
        //[InlineData("Inception 8 10", "Inception", 8, 10)]
        //[InlineData("Inception810", false, "", 0, 0)]
        //public void TryParseTheaterInputTest(string input, bool expectedCanParse, string expectedTitle, int expectedRow, int expectedSeatPerRow)
        //{
        //    TheaterDetail output = _inputParserService.TryParseTheaterInput(input);

        //    Assert.NotNull(output);
        //    Assert.Equal(expectedTitle, output.Title);
        //    Assert.Equal(expectedRow, output.Row);
        //    Assert.Equal(expectedSeatPerRow, output.SeatPerRow);
        //}
    }

    class TryParseTheaterInputData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Inception 8 10", new TheaterDetail { Title = "Inception", Row = 8, SeatPerRow = 10 }};
            yield return new object[] { "Inception810", null };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
