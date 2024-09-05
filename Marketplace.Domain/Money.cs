using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Money : Value<Money>
    {
        public decimal Amount { get; }
        public Money(decimal amount) => Amount = amount;
    }
}
