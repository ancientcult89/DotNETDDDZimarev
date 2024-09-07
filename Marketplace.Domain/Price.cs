namespace Marketplace.Domain
{
    public class Price : Money
    {
        public Price(decimal amount, string currencyCode, ICurrencyLookUp currencyLookUp) : base(amount, currencyCode, currencyLookUp)
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be negative", nameof(amount));
            
        }
    }
}
