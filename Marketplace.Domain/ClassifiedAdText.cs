namespace Marketplace.Domain
{
    public class ClassifiedAdText
    {
        public static ClassifiedAdText FromString(string title) => new ClassifiedAdText(title);
        private readonly string _value;
        public ClassifiedAdText(string value)
        {
            //if (value.Length > 100)
            //    throw new ArgumentException("Title cannot be longer that 100 characters", nameof(value));

            _value = value;
        }
    }
}
