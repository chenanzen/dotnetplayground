using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC
{
    internal static class GICConstants
    {
        public static readonly string MainMenu =
        "Welcome to AwesomeGIC Bank! What would you like to do?\n" +
        "[T] Input transactions\n" +
        "[I] Define interest rules\n" +
        "[P] Print statement\n" +
        "[Q] Quit\n" +
        "> ";

        public static readonly string TransactionMenu =
        "Please enter transaction details in <Date> <Account> <Type> <Amount> format\n" +
        "(or enter blank to go back to main menu):\n" +
        "> ";

        public static readonly string DefineInterestMenu =
            "Please enter interest rules details in <Date> <RuleId> <Rate in %> format \n" +
            "(or enter blank to go back to main menu):\n" +
            "> ";

        public static readonly string PrintStatementMenu =
            "Please enter account and month to generate the statement <Account> <Year><Month>\n" +
            "(or enter blank to go back to main menu):\n" +
            "> ";

        public static readonly string ExitMessage =
            "Thank you for banking with AwesomeGIC Bank.\n" +
            "Have a nice day!";

        public static readonly string InputYearMonthFormat = "yyyyMM";
        public static readonly string InputDateTimeFormat = "yyyyMMdd";
        public static readonly string OutputDateTimeFormat = "yyyyMMdd";

        public static readonly string TransactionIdentityFormat = "00";
        public static readonly string CurrencyOutputFormat = "0.00";

        public static readonly decimal DefaultInterest = 2.2m;
    }

    public enum TransactionType
    {
        /// <summary>
        /// Withdrawal
        /// </summary>
        W,

        /// <summary>
        /// Deposit
        /// </summary>
        D,

        /// <summary>
        /// Interest
        /// </summary>
        I

    }
}
