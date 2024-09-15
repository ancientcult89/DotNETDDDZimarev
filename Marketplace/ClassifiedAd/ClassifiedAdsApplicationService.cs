using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;

namespace Marketplace.ClassifiedAd
{
    public class ClassifiedAdsApplicationService : IApplicationService
    {
        private readonly ICurrencyLookUp _currencyLookUp;
        private readonly IAggregateStore _aggregateStore;

        public ClassifiedAdsApplicationService(ICurrencyLookUp currencyLookUp, IAggregateStore aggregateStore)
        {
            _currencyLookUp = currencyLookUp;
            _aggregateStore = aggregateStore;
        }

        public Task Handle(object command) =>
            command switch
            {
                V1.Create cmd =>
                    HandleCreate(cmd),
                V1.SetTitle cmd =>
                    HandleUpdate(cmd.Id, c => c.SetTitle(ClassifiedAdTitle.FromString(cmd.Title))),
                V1.UpdateText cmd =>
                    HandleUpdate(cmd.Id, c => c.UpdateText(ClassifiedAdText.FromString(cmd.Text))),
                V1.UpdatePrice cmd =>
                    HandleUpdate(cmd.Id, c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookUp))),
                V1.RequestToPublish cmd =>
                    HandleUpdate(cmd.Id, c => c.RequestToPublish()),
                _ => Task.CompletedTask
            };


        private async Task HandleCreate(V1.Create cmd)
        {
            if (await _aggregateStore.Exists<Domain.ClassifiedAd.ClassifiedAd, ClassifiedAdId>(new ClassifiedAdId(cmd.Id)))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var classifiedAd = new Marketplace.Domain.ClassifiedAd.ClassifiedAd(
                new ClassifiedAdId(cmd.Id),
                new UserId(cmd.OwnerId)
            );
            await _aggregateStore.Save<Domain.ClassifiedAd.ClassifiedAd, ClassifiedAdId>(classifiedAd);
        }

        private async Task HandleUpdate(Guid classifiedAdId, Action<Marketplace.Domain.ClassifiedAd.ClassifiedAd> update) =>
            this.HandleUpdate(_aggregateStore, new ClassifiedAdId(classifiedAdId), update);
    }
}
