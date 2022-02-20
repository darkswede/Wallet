using FluentAssertions;
using System;
using System.Linq;
using Wallet.Core.Models;
using Xunit;

namespace Wallet.Tests.Unit.Models
{
    public class FinanceWalletTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void CreateWallet_WhenInvalidNameProvided_ShouldThrowError(string name)
        {
            Action act = () => new FinanceWallet(name);

            act.Should().Throw<Exception>().WithMessage("wallet name cannot be empty");
        }

        [Fact]
        public void AddToken_WhenAddMethodCalled_ShouldAddTokenToWallet()
        {
            var expectedToken = Token.Create(1, "usd", 1, 1, 1);
            var wallet = new FinanceWallet("wallet");

            wallet.AddToken(1, "usd", 1, 1, 1);

            var result = wallet.Tokens.SingleOrDefault(x => x.Name.Contains(expectedToken.Name));
            result.FinanceWalletID.Should().Be(expectedToken.FinanceWalletID);
            result.Name.Should().BeEquivalentTo(expectedToken.Name);
            result.Amount.Should().Be(expectedToken.Amount);
            result.PurchaseValue.Should().Be(expectedToken.PurchaseValue);
            result.CurrentTokenValue.Should().Be(expectedToken.CurrentTokenValue);
        }

        [Fact]
        public void RemoveToken_WhenExistInWallet_ShouldRemoveTokenFromWallet()
        {
            var wallet = new FinanceWallet("wallet");
            wallet.AddToken(1, "usd", 1, 1, 1);

            wallet.RemoveToken("usd");

            var result = wallet.Tokens.SingleOrDefault(x => x.Name == "usd");
            result.Should().BeNull();
        }

        [Fact]
        public void IncreaseWorth_WhenValidAmountAdded_ShouldIncreaseWorth()
        {
            var expected = 99.4M;
            var wallet = new FinanceWallet("wallet");

            wallet.StartWorthIncrease(99.4M);

            wallet.StartWorth.Should().Be(expected);
        }

        [Fact]
        public void IncreaseWorth_WhenInValidAmountAdded_ShouldThrowException()
        {
            var wallet = new FinanceWallet("wallet");

            Action act = () => wallet.StartWorthIncrease(-99.4M);

            act.Should().Throw<Exception>("value cannot be negative");
        }

        [Fact]
        public void DecreaseWorth_WhenValidAmountAdded_ShouldDecreaseWorth()
        {
            var expected = 99.4M;
            var wallet = new FinanceWallet("wallet");

            wallet.StartWorthIncrease(199.4M);
            wallet.StartWorthDecrease(100M);

            wallet.StartWorth.Should().Be(expected);
        }

        [Fact]
        public void DecreaseWorth_WhenInValidAmountAdded_ShouldThrowException()
        {
            var wallet = new FinanceWallet("wallet");

            Action act = () => wallet.StartWorthDecrease(-3459.4M);

            act.Should().Throw<Exception>("value cannot be negative");
        }

        [Fact]
        public void CalculateCurrentTotalWorth_WhenValidDataProvided_ShouldCalculateWorth()
        {
            var expectedWorth = 906.24M;
            var wallet = new FinanceWallet("wallet");
            wallet.AddToken(1, "usd", 512, 1, 1.77M);

            wallet.CurrentTotalWorth.Should().Be(expectedWorth);
        }
    }
}
