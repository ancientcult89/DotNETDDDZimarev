using Marketplace.Domain.Shared;

namespace Marketplace.Domain.UserProfile
{
    public class DisplayName
    {
        public string Value { get; }

        internal DisplayName(string value) => Value = value;

        public static DisplayName FromString(string displayName, CheckTextForProfanity hasProfanity)
        {
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException(nameof(displayName));
            if (hasProfanity(displayName))
                throw new DomainExceptions.ProfanityFound(displayName);

            return new DisplayName(displayName);
        }

        public static implicit operator string(DisplayName fullName) => fullName.Value;

        protected DisplayName() { }
    }
}
