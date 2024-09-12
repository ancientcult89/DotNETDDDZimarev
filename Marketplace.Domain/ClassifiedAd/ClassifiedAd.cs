using Marketplace.Domain.Shared;
using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd
{
    public class ClassifiedAd : AggregateRoot<ClassifiedAdId>
    {
        //RavenDB key
        private string DbId
        {
            get => $"ClassifiedAd/{Id.Value}";
            set { }
        }

        //EFCore key
        public Guid ClassifiedAdId { get; private set; }

        public ClassifiedAdId Id { get; private set; }
        public List<Picture> Pictures { get; private set; }

        public ClassifiedAd() { }
        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            Pictures = new List<Picture>();
            Apply(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });
        }

        public void CreateClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            var classifiedId = new ClassifiedAd(id, ownerId);
            //store the entity somehow
        }

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            Apply(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });
        }
        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            Apply(new Events.ClassifiedAdTextUpdated
            {
                Id = Id,
                AdText = text
            });
        }
        public void UpdatePrice(Price price)
        {
            Price = price;
            Apply(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                Price = price.Amount,
                CurrencyCode = price.Currency
            });
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingRevew;
            Apply(new Events.ClassifiedAdSentForReview
            {
                Id = Id
            });
        }

        private Picture FindPicture(PictureId pictureId) => Pictures.FirstOrDefault(x => x.Id == pictureId);

        public void ResizePicture(PictureId pictureId, PictureSize newSize)
        {
            var picture = FindPicture(pictureId);
            if (picture == null)
                throw new InvalidOperationException("Cannot resize a picture that i dont have");

            picture.Resize(newSize);
        }

        private Picture FirstPicture => Pictures.OrderBy(x => x.Order).FirstOrDefault();

        protected override void EnsureValidState()
        {
            var valid =
                Id != null &&
                OwnerId != null &&
                State switch
                {
                    ClassifiedAdState.PendingRevew =>
                        Title != null
                        && Text != null
                        && Price?.Amount > 0
                        && FirstPicture.HasCorrectSize(),
                    ClassifiedAdState.Active =>
                        Title != null
                        && Text != null
                        && Price?.Amount > 0
                        && FirstPicture.HasCorrectSize()
                        && ApprovedBy != null,
                    _ => true
                };

            if (!valid)
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    Title = ClassifiedAdTitle.NoTitle;
                    Text = ClassifiedAdText.NoText;
                    Price = Price.NoPrice;
                    ApprovedBy = UserId.NoUser;
                    //required for persistance
                    ClassifiedAdId = e.Id;
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    Text = new ClassifiedAdText(e.AdText);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingRevew;
                    break;
                case Events.PictureAddedToAClassifiedAd e:
                    var newPicture = new Picture(Apply);
                    ApplyToEntity(newPicture, e);
                    Pictures.Add(newPicture);
                    break;
            }
        }

        public UserId OwnerId { get; private set; }
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

        public void AddPicture(Uri pictureUri, PictureSize size) =>
            Apply(new Events.PictureAddedToAClassifiedAd
            {
                PictureId = new Guid(),
                ClassifiedAdId = Id,
                Url = pictureUri.ToString(),
                Height = size.Height,
                Width = size.Width,
                Order = Pictures.Max(x => x.Order),
            });
    }
}
