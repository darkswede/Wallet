namespace Wallet.Core
{
    public static class PercentCalculator
    {
        public static decimal CalculateChange(decimal previous, decimal current)
        {
            if (previous == 0)
            {
                return 0;
            }

            if (current == 0)
            {
                return -100;
            }

            var change = ((current - previous) / previous) * 100;

            return change;
        }
    }
}
