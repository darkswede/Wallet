using System;
using System.Collections.Generic;
using System.Linq;

namespace Wallet.Core.Models
{
    public class FinanceWallet
    {
        private ISet<Token> _tokens = new HashSet<Token>();

        public Guid ID { get; set; }
        public string Name { get; protected set; }
        public decimal StartWorth { get; private set; }
        public decimal CurrentTotalWorth { get { return CurrentTotalWorthCalculator(); } }
        public decimal PercentChange { get { return PercentCalculator.CalculateChange(StartWorth, CurrentTotalWorth); } }
        public IEnumerable<Token> Tokens
        {
            get { return _tokens; }
            set { _tokens = new HashSet<Token>(value); }
        }

        public FinanceWallet(string name)
        {
            ID = Guid.NewGuid();
            SetName(name);
        }

        public void SetName(string name)
        {
            if (Name == name)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("wallet name cannot be empty");
            }

            Name = name;
        }

        public void AddToken(string name, decimal amount, decimal purchasePrice)
        {
            _tokens.Add(Token.Create(name, amount, purchasePrice));
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

        private void StartWorthIncrease(decimal value)
        {
            if (value < 0)
            {
                throw new Exception("value cannot be negative");
            }

            StartWorth += value;
        }

        private void StartWorthDecrease(decimal value)
        {
            if (value < 0)
            {
                throw new Exception("value cannot be negative");
            }

            StartWorth -= value;
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
