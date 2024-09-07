
namespace Marketplace.Domain
{
    public class ClassifiedAd
    {
        public ClassifiedAdId Id { get; private set; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            EnsureValidState();
        }

        public void CreateClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            var classifiedId = new ClassifiedAd(id, ownerId);
            //store the entity somehow
        }

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();
        }
        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();
        } 
        public void UpdatePrice(Price price) {
            Price = price;
            EnsureValidState();
        } 

        public void RequestToPublish()
        {
            if (Title == null)
                throw new InvalidEntityStateException(this, "tittle cannot be empty");
            if (Text == null)
                throw new InvalidEntityStateException(this, "text cannot be empty");
            if (Price?.Amount == 0)
                throw new InvalidEntityStateException(this, "price cannot be zero");

            State = ClassifiedAdState.PendingRevew;
            EnsureValidState();
        }

        protected void EnsureValidState()
        {
            var valid =
                Id != null &&
                OwnerId != null &&
                (State switch
                {
                    ClassifiedAdState.PendingRevew =>
                        Title != null
                        && Text != null
                        && Price?.Amount > 0,
                    ClassifiedAdState.Active =>
                        Title != null
                        && Text != null
                        && Price?.Amount > 0
                        && ApprovedBy != null,
                    _ => true
                });

            if (!valid)
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
        }

        public UserId OwnerId { get; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }

        public enum ClassifiedAdState
        {
            PendingRevew,
            Active,
            Inactive,
            MarkedAsSold,
        }
    }
}
