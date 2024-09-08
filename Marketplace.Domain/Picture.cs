using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Picture : Entity<PictureId>
    {
        public Picture(Action<object> applier) : base(applier)
        {
        }

        internal PictureSize Size { get; set; }
        internal Uri Location { get; set; }
        internal int Order { get; set; }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToAClassifiedAd e:
                    Id = new PictureId(e.PictureId);
                    Location = new Uri(e.Url);
                    Size = new PictureSize { Height = e.Height, Width = e.Width };
                    Order = e.Order;
                    break;
                case Events.ClassifiedAdPictureResized e:
                    Size = new PictureSize { Height = e.Height, Width = e.Width};
                    break;
            }
        }

        internal void Resize(PictureSize newSize) => Apply(new Events.ClassifiedAdPictureResized
        {
            PictureId = Id.Value,
            Height = newSize.Height,
            Width = newSize.Width
        });
    }
}
