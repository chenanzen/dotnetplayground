using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AwesomeGIC
{

    public interface IGICDataAccess
    {
        public void SetRunningState(bool newRunningState);
        public bool GetRunningState();
        void Transact(DateTime transactionDateTime, string accountName, TransactionType transactionType, decimal amount);
        void UpdateInterestSetting(DateTime transactionDateTime, string settingName, decimal value);
        GICAccount GetAccount(string accountName);
        Dictionary<DateTime, GICInterestSetting> GetInterestSetting();
    }

    public class GICData: IGICDataAccess
    {
        /// <summary>
        /// true = running; false = stopping
        /// </summary>
        private bool _currentRunningState;

        private Dictionary<string, GICAccount> _accounts;
        private Dictionary<DateTime, GICInterestSetting> _interestSettings;

        /// <summary>
        /// 
        /// </summary>
        public GICData()
        {
            _currentRunningState = true;
            _accounts = new Dictionary<string, GICAccount>();
            _interestSettings = new Dictionary<DateTime, GICInterestSetting>();
        }

        public bool GetRunningState()
        {
            return _currentRunningState;
        }

        public void SetRunningState(bool newstate)
        {
            _currentRunningState = newstate;
        }

        public void Transact(DateTime transactionDateTime, string accountName, TransactionType transactionType, decimal amount)
        {
            GICAccount? account;
            var accountExist = _accounts.TryGetValue(accountName, out account);
            if (!accountExist)
            {
                account = new GICAccount(accountName);
                _accounts[accountName] = account;
            }

            account?.Transact(transactionDateTime, transactionType, amount);
        }

        public GICAccount GetAccount(string accountName)
        {
            GICAccount? account;
            var accountExist = _accounts.TryGetValue(accountName, out account);
            if (!accountExist)
            {
                account = new GICAccount(accountName);
                _accounts[accountName] = account;
            }

            return account!;
        }

        public void UpdateInterestSetting(DateTime transactionDateTime, string settingName, decimal value)
        {
            _interestSettings[transactionDateTime] = new GICInterestSetting(transactionDateTime, settingName, value);
        }

        public Dictionary<DateTime, GICInterestSetting> GetInterestSetting()
        {
            return _interestSettings;
        }
    }

    public class GICAccount
    {
        private string _accountName;
        private decimal _balance;
        private List<GICTransaction> _transactions;

        public string AccountName
        {
            get { return _accountName; } 
        }

        public List<GICTransaction> Transactions
        {
            get { return _transactions; }
        }

        public GICAccount(string accountName) 
        { 
            _accountName = accountName;
            _balance = 0;
            _transactions = new List<GICTransaction>();
        }

        public void Transact(DateTime transactionDateTime, TransactionType transactionType, decimal amount)
        {
            var id = _transactions.Where(t => t.TransactionDateTime == transactionDateTime).Count() + 1;
            var newTransactionId = $"{transactionDateTime.ToString(GICConstants.InputDateTimeFormat)}-{id.ToString(GICConstants.TransactionIdentityFormat)}";

            if (transactionType == TransactionType.W)
            {
                // withdrawal
                var newBalance = _balance - amount;
                if (newBalance >= 0)
                {
                    _balance = newBalance;
                    _transactions.Add(new GICTransaction(newTransactionId, transactionType, transactionDateTime, amount));
                }
            }
            else if (transactionType == TransactionType.D)
            {
                // deposit
                _balance += amount;
                _transactions.Add(new GICTransaction(newTransactionId, transactionType, transactionDateTime, amount));
            }
        }
    }

    public class GICTransaction
    {
        public string TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public decimal Amount { get; set; }

        public GICTransaction(string transactionId, TransactionType transactionType, DateTime transactionDateTime, decimal amount)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
            TransactionDateTime = transactionDateTime;
            Amount = amount;
        }
    }

    public class GICInterestSetting
    {
        public DateTime InterestSettingDateTime { get; private set; }
        public string InterestSettingName { get; private set; }
        public decimal InterestSettingValue { get; private set; }

        public GICInterestSetting(DateTime interestSettingDateTime, string interestSettingName, decimal interestSettingValue)
        {
            InterestSettingDateTime = interestSettingDateTime;
            InterestSettingName = interestSettingName;
            InterestSettingValue = interestSettingValue;
        }
    }

    public class GICStatement
    {
        public string TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

        public GICStatement(string transactionId, TransactionType transactionType, DateTime transactionDateTime, decimal amount, decimal balance)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
            TransactionDateTime = transactionDateTime;
            Amount = amount;
            Balance = balance;
        }
    }
}
