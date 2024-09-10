namespace Marketplace.Domain
{
    public class Price : Money
    {
        private Price(decimal amount, string currencyCode, ICurrencyLookUp currencyLookup)
            : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 0)
                throw new ArgumentException(
                    "Price cannot be negative",
                    nameof(amount));
        }

        internal Price(decimal amount, string currencyCode)
            : base(amount, new Currency { CurrencyCode = currencyCode })
        {
        }

        public static Price NoPrice = new Price(0, "");

        public new static Price FromDecimal(decimal amount, string currency,
            ICurrencyLookUp currencyLookup) =>
            new Price(amount, currency, currencyLookup);

        // Satisfy the serialization requirements 
        protected Price() { }
    }
}
