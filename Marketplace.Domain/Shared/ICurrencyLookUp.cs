using Marketplace.Framework;

namespace Marketplace.Domain.Shared
{
    public interface ICurrencyLookUp
    {
        Currency FindCurrency(string currencyCode);
    }

    public class Currency : Value<Currency>
    {
        public string CurrencyCode { get; set; }
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }

        public static Currency None = new Currency { InUse = false };
        public static implicit operator string(Currency currency) => currency.CurrencyCode;
    }
}
