namespace Marketplace.Domain
{
    public class ClassifiedAdId
    {
        private readonly Guid _value;

        public ClassifiedAdId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException("Classified Ad id cannot be empty", nameof(value));

            _value = value;
        }
    }
}
