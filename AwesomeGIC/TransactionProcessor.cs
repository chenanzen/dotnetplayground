using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC
{
    public class TransactionProcessor : IProcessor
    {
        private readonly IIOService _ioService;
        private readonly IGICDataAccess _gicDataAccess;

        public TransactionProcessor(IIOService ioService, IGICDataAccess gicDataAccess)
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
                    // sample: 20230626 AC001 W 100.00
                    // Date should be in YYYYMMdd format
                    // Account is a string, free format
                    // Type is D for deposit, W for withdrawal, case insensitive
                    // Amount must be greater than zero, decimals are allowed up to 2 decimal places
                    // An account's balance should not be less than 0. Therefore, the first transaction on an account should not be a withdrawal, and any transactions thereafter should not make balance go below 0
                    // Each transaction should be given a unique id in YYYMMdd - xx format, where xx is a running number(see example below)
                    var inputToken = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (inputToken.Length == 4)
                    {
                        var dateString = inputToken[0];
                        var accountName = inputToken[1];
                        var transactionTypeString = inputToken[2];
                        var amountString = inputToken[3];

                        var isValidDate = DateTime.TryParseExact(dateString, GICConstants.InputDateTimeFormat, 
                            System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                            out DateTime inputDate);

                        var isValidTransactionType = Enum.TryParse<TransactionType>(transactionTypeString, ignoreCase: true, out TransactionType transactionType);

                        var isValidAmount = decimal.TryParse(amountString, out decimal amount);

                        if (isValidDate && isValidTransactionType && isValidAmount)
                        {
                            _gicDataAccess.Transact(inputDate, accountName, transactionType, amount);

                            // print transaction
                            var accountToPrint = _gicDataAccess.GetAccount(accountName);
                            _ioService.PrintStatement(accountToPrint);
                        }
                    }
                }
            }
        }
    }
}
