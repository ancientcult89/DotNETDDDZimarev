using Marketplace.Domain.Shared;

namespace Marketplace.Tests
{
    public class MoneyTests
    {
        private static readonly ICurrencyLookUp CurrencyLookUp = new FakeCurrencyLookup();

        [Fact]
        public void Money_objects_with_the_same_amount_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookUp);
            var secondAmount = Money.FromDecimal(5, "EUR", CurrencyLookUp);
            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void Two_of_same_amounts_but_differentCurrencies_should_not_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookUp);
            var secondAmount = Money.FromDecimal(5, "USD", CurrencyLookUp);
            Assert.NotEqual(firstAmount, secondAmount);
        }

        [Fact]
        public void FromString_and_FromDecimal_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookUp);
            var secondAmount = Money.FromString("5,00", "EUR", CurrencyLookUp);
            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void Sum_of_money_gives_full_amount()
        {
            var coin1 = Money.FromDecimal(1, "EUR", CurrencyLookUp);
            var coin2 = Money.FromDecimal(2, "EUR", CurrencyLookUp);
            var coin3 = Money.FromDecimal(2, "EUR", CurrencyLookUp);
            var banknote = Money.FromDecimal(5, "EUR", CurrencyLookUp);
            Assert.Equal(banknote, coin1 + coin2 + coin3);
        }

        [Fact]
        public void Unused_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(100, "DEM", CurrencyLookUp));
        }

        [Fact]
        public void Unknown_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(100, "WHAT?", CurrencyLookUp));
        }

        [Fact]
        public void Throw_then_too_many_decimal_places()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Money.FromDecimal(100.123m, "EUR", CurrencyLookUp));
        }

        [Fact]
        public void Throws_on_adding_different_currencies()
        {
            var firstAmount = Money.FromDecimal(5, "USD", CurrencyLookUp);
            var secondAmount = Money.FromDecimal(5, "EUR", CurrencyLookUp);
            Assert.Throws<CurrencyMismatchException>(() => firstAmount + secondAmount);
        }

        [Fact]
        public void Throws_on_substracting_different_currencies()
        {
            var firstAmount = Money.FromDecimal(5, "USD", CurrencyLookUp);
            var secondAmount = Money.FromDecimal(5, "EUR", CurrencyLookUp);
            Assert.Throws<CurrencyMismatchException>(() => firstAmount - secondAmount);
        }
    }
}
