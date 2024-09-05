namespace Marketplace.Domain
{
    //Value object
    public class UserId
    {
        private readonly Guid _value;

        public UserId(Guid value)
        {
            if(value == default)
                throw new ArgumentNullException(nameof(value), "User id cannot be empty");

            _value = value;
        }
    }
}
