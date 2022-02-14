using System;

namespace Wallet.Core.Models
{
    public class Token
    {
        public Guid Id { get; }
        public string Name { get; protected set; }
        public decimal Amount { get; protected set; }
        public decimal PurchasePrice { get; protected set; }
        public decimal PurchaseValue { get { return Amount * PurchasePrice; } }
        public decimal CurrentTokenValue { get; protected set; }
        public decimal TotalValue { get { return Amount * CurrentTokenValue; } }
        public decimal PercentChange { get { return PercentCalculator.CalculateChange(PurchaseValue, TotalValue); } }

        private Token(string name, decimal amount, decimal purchasePrice)
        {
            Id = Guid.NewGuid();
            SetName(name);
            IncreaseAmount(amount);
            SetPurchasePrice(purchasePrice);
        }

        public static Token Create(string name, decimal amount, decimal purchaseValue) => new Token(name, amount, purchaseValue);

        public void SetName(string name)
        {
            if (Name == name)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("enter name");
            }

            Name = name;
        }

        public void IncreaseAmount(decimal amount)
        {
            ValidateAmount(amount);

            Amount += amount;
        }

        public void DecreaseAmount(decimal amount)
        {
            ValidateAmount(amount);

            Amount -= amount;
        }

        public void SetPurchasePrice(decimal value)
        {
            ValidatePurchasePrice(value);

            PurchasePrice = value;
        }

        public void UpdatePurchasePrice(decimal value)
        {
            ValidatePurchasePrice(value);

            var current = (PurchasePrice + value) / 2;
            PurchasePrice = current;
        }

        public void SetCurrentTokenValue(decimal value)
        {
            if (CurrentTokenValue == value)
            {
                return;
            }

            if (value < 0)
            {
                throw new Exception("no negative values allowed");
            }

            CurrentTokenValue = value;
        }

        private void ValidateAmount(decimal value)
        {
            if (Amount == value)
            {
                return;
            }

            if (value <= 0)
            {
                throw new Exception("you have to deduct more than 0");
            }
        }

        private void ValidatePurchasePrice(decimal value)
        {
            if (PurchasePrice == value)
            {
                return;
            }

            if (value < 0)
            {
                throw new Exception("no negative values. in case of airdrop - enter 0 as a price");
            }
        }
    }
}
