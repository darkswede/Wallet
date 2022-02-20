using System;
using System.Collections.Generic;
using System.Linq;

namespace Wallet.Core.Models
{
    public class FinanceWallet
    {
        private ISet<Token> _tokens = new HashSet<Token>();

        public int ID { get; set; }
        public string Name { get; protected set; }
        public decimal StartWorth { get; protected set; }
        public decimal CurrentTotalWorth { get { return CurrentTotalWorthCalculator(); } }
        public decimal PercentChange { get { return PercentCalculator.CalculateChange(StartWorth, CurrentTotalWorth); } }
        public IEnumerable<Token> Tokens
        {
            get { return _tokens; }
            set { _tokens = new HashSet<Token>(value); }
        }

        public FinanceWallet(string name)
        {
            SetName(name);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("wallet name cannot be empty");
            }

            if (Name == name)
            {
                return;
            }

            Name = name;
        }

        public void AddToken(int financeWalletID, string name, decimal amount, decimal purchaseValue, decimal currentTokenValue)
        {
            _tokens.Add(Token.Create(financeWalletID, name, amount, purchaseValue, currentTokenValue));
        }

        public void RemoveToken(string name)
        {
            var token = _tokens.SingleOrDefault(x => x.Name == name);
            if (token == null)
            {
                return;
            }

            _tokens.Remove(token);
        }

        public void StartWorthIncrease(decimal value)
        {
            ValidateAmount(value);

            StartWorth += value;
        }

        public void StartWorthDecrease(decimal value)
        {
            ValidateAmount(value);

            StartWorth -= value;
        }

        private void ValidateAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new Exception("value cannot be negative");
            }
        }

        private decimal CurrentTotalWorthCalculator()
        {
            var walletValue = 0M;
            foreach (var token in _tokens)
            {
                var value = token.TotalValue;
                walletValue += value;
            }

            return walletValue;
        }
    }
}
