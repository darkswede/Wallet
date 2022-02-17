using Microsoft.EntityFrameworkCore;
using Wallet.Core.Models;

namespace Wallet.Infrastructure.DatabaseContext
{
    public class WalletContext : DbContext
    {
        public WalletContext(DbContextOptions<WalletContext> options) : base(options)
        {
        }

        public DbSet<FinanceWallet> Wallets { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
