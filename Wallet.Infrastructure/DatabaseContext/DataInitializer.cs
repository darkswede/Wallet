using System.Linq;
using Wallet.Core.Models;

namespace Wallet.Infrastructure.DatabaseContext
{
    public static class DataInitializer
    {
        public static void Initialize(WalletContext context)
        {
            context.Database.EnsureCreated();

            if (context.Wallets.Any())
            {
                return;
            }

            context.Wallets.Add(new FinanceWallet("Stable Coins"));
            context.SaveChanges();

            if (context.Tokens.Any())
            {
                return;
            }

            var tokens = new Token[]
            {
                Token.Create("USDT", 0, 0),
                Token.Create("USDC", 0, 0),
                Token.Create("BUSD", 0, 0)
            };
            foreach (var token in tokens)
            {
                context.Tokens.Add(token);
            }
            context.SaveChanges();
        }
    }
}
