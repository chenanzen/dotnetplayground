using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AwesomeGIC
{
    public class DefineInterestProcessor : IProcessor
    {
        private readonly IIOService _ioService;
        private readonly IGICDataAccess _gicDataAccess;

        public DefineInterestProcessor(IIOService ioService, IGICDataAccess gicDataAccess)
        {
            _ioService = ioService;
            _gicDataAccess = gicDataAccess;
        }

        public void Process()
        {
            bool keepPrompting = true;
            while (keepPrompting)
            {
                var input = _ioService.GetInput(GICConstants.TransactionMenu);

                if (string.IsNullOrEmpty(input)) keepPrompting = false;
                else
                {
                    // validate and parse input
                    // sample: 20230615 RULE03 2.20
                    // Some constraints to note:
                    // Date should be in YYYYMMdd format
                    // RuleId is string, free format
                    // Interest rate should be greater than 0 and less than 100
                    // If there's any existing rules on the same day, the latest one is kept
                    var inputToken = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (inputToken.Length == 3)
                    {
                        var dateString = inputToken[0];
                        var settingName = inputToken[1];
                        var valueString = inputToken[2];

                        var isValidDate = DateTime.TryParseExact(dateString, GICConstants.InputDateTimeFormat,
                            System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                            out DateTime inputDate);

                        var isValidAmount = decimal.TryParse(valueString, out decimal value);

                        if (isValidDate && isValidAmount)
                        {
                            _gicDataAccess.UpdateInterestSetting(inputDate, settingName, value);

                            // print transaction
                            var interestSettingToPrint = _gicDataAccess.GetInterestSetting();
                            _ioService.PrintInterestSetting(interestSettingToPrint);
                        }
                    }
                }
            }
        }
    }
}
