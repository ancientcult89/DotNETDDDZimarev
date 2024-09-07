namespace Marketplace.Domain
{
    public class ClassifiedAdText
    {
        public static ClassifiedAdText FromString(string title) => new ClassifiedAdText(title);
        private readonly string _value;
        internal ClassifiedAdText(string value)
        {
            _value = value;
        }
        public static implicit operator string(ClassifiedAdText text) => text._value;
    }
}
