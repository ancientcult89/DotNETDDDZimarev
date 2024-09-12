using Marketplace.Framework;

namespace Marketplace.Domain.UserProfile
{
    public class FullName : Value<FullName>
    {
        public string Value { get; }

        internal FullName(string value) => Value = value;

        public static FullName FromString(string fullName)
        {
            if(string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException(nameof(fullName));

            return new FullName(fullName);
        }

        public static implicit operator string(FullName fullName) => fullName.Value;

        protected FullName() { }
    }
}
