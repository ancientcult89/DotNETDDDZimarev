using Marketplace.Framework;

namespace Marketplace.Domain
{
    //Value object
    public class UserId : Value<UserId>
    {
        private readonly Guid _value;

        public UserId(Guid value) => _value = value;
    }
}
