using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AwesomeGIC
{
    public interface IIOService
    {
        string GetInput(string message);
        void ShowMessage(string message);
        void PrintStatement(GICAccount account);
        void PrintStatement(string accountName, List<GICStatement> transactions);
        void PrintInterestSetting(Dictionary<DateTime, GICInterestSetting> interestSettings);
    }

    public class ConsoleIOService : IIOService
    {
        public string GetInput(string message)
        {
            Console.Write(message);
            var input = Console.ReadLine();
            input = input ?? string.Empty;

            return input;
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
            return;
        }

        /// <summary>
        /// print account statement in following format
        /// Account: AC001
        /// | Date     | Txn Id      | Type | Amount |
        /// | 20230505 | 20230505-01 | D    | 100.00 |
        /// | 20230601 | 20230601-01 | D    | 150.00 |
        /// | 20230626 | 20230626-01 | W    |  20.00 |
        /// | 20230626 | 20230626-02 | W    | 100.00 |
        /// </summary>
        /// <param name="account"></param>
        public void PrintStatement(GICAccount account)
        {
            var output =
            $"Account: {account.AccountName}\n" +
            $"| Date     | Txn Id      | Type | Amount |\n";

            var transactions = account.Transactions.OrderBy(t => t.TransactionDateTime).ToList();
            foreach (var transaction in transactions)
            {
                output += $"| {transaction.TransactionDateTime.ToString(GICConstants.InputDateTimeFormat)} " +
                    $"| {transaction.TransactionId} | {transaction.TransactionType.ToString()}    | {transaction.Amount.ToString(GICConstants.CurrencyOutputFormat)} |\n";
            }

            Console.WriteLine(output);
        }


        /// <summary>
        /// print interest setting in following sample format
        /// Interest rules:
        /// | Date     | RuleId | Rate(%) |
        /// | 20230101 | RULE01 |    1.95 |
        /// | 20230520 | RULE02 |    1.90 |
        /// | 20230615 | RULE03 |    2.20 |
        /// </summary>
        /// <param name="interestSettings"></param>
        public void PrintInterestSetting(Dictionary<DateTime, GICInterestSetting> interestSettings)
        {
            var maxstringLength = interestSettings.Max(s => s.Value.InterestSettingName.Length);

            var output =
            $"Interest rules:\n" +
            $"| Date     | {"RuleId".PadRight(maxstringLength)} | Rate(%) |\n";

            var keys = interestSettings.Keys.OrderBy(k => k).ToList();
            foreach (var key in keys)
            {
                var setting = interestSettings[key];
                var interest = setting.InterestSettingValue.ToString(GICConstants.CurrencyOutputFormat);
                output += $"| {setting.InterestSettingDateTime.ToString(GICConstants.InputDateTimeFormat)} | {setting.InterestSettingName.PadRight(maxstringLength)} | {interest.PadLeft(7)} |\n";
            }

            Console.WriteLine(output);
        }

        public void PrintStatement(string accountName, List<GICStatement> transactions)
        {
            var maxstringLength = transactions.Max(s => s.TransactionId.Length);

            var output =
            $"Account: {accountName}\n" +
            $"| Date     | Txn Id      | Type | Amount  | Balance |\n";

            transactions = transactions.OrderBy(t => t.TransactionDateTime).ToList();
            foreach (var transaction in transactions)
            {
                output += $"| {transaction.TransactionDateTime.ToString(GICConstants.InputDateTimeFormat)} " +
                    $"| {transaction.TransactionId.PadRight(maxstringLength)} | {transaction.TransactionType.ToString()}    " + 
                    $"| {transaction.Amount.ToString(GICConstants.CurrencyOutputFormat).PadLeft(7)} | {transaction.Balance.ToString(GICConstants.CurrencyOutputFormat).PadLeft(7)} |\n";
            }

            Console.WriteLine(output);
        }
    }
}
