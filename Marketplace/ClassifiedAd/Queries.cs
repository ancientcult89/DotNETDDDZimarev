using Dapper;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Data.Common;
using static Marketplace.ClassifiedAd.QueryModels;
using static Marketplace.ClassifiedAd.ReadModels;
using static Marketplace.Domain.ClassifiedAd.ClassifiedAd;

namespace Marketplace.ClassifiedAd
{
    public static class Queries
    {
        public static Task<List<PublicClassifiedAdListItem>> Query(this IAsyncDocumentSession session, QueryModels.GetPublishedClassifiedAds query) =>
            session.Query<Domain.ClassifiedAd.ClassifiedAd>().Where(x => x.State == ClassifiedAdState.Active).Select(x => new PublicClassifiedAdListItem
            {
                ClassifiedAdId = x.Id.Value,
                Price = x.Price.Amount,
                Title = x.Title.Value,
                CurrencyCode = x.Price.Currency.CurrencyCode,
            })
            .PagedList(query.Page, query.PageSize);

        public static Task<List<PublicClassifiedAdListItem>> Query(this IAsyncDocumentSession session, QueryModels.GetOwnersClassifiedAd query) =>
            session.Query<Domain.ClassifiedAd.ClassifiedAd>().Where(x => x.OwnerId == query.OwnerId).Select(x => new PublicClassifiedAdListItem
            {
                ClassifiedAdId = x.Id.Value,
                Price = x.Price.Amount,
                Title = x.Title.Value,
                CurrencyCode = x.Price.Currency.CurrencyCode,
            })
            .Skip(query.Page * query.PageSize)
            .Take(query.PageSize)
            .PagedList(query.Page, query.PageSize);

        //public static Task<ClassifiedAdDetails> Query(this IAsyncDocumentSession session, QueryModels.GetPublicClassifiedAd query) =>
        //    (
        //        from ad in session.Query<Domain.ClassifiedAd.ClassifiedAd>()
        //        where ad.Id.Value == query.ClassifiedAdId
        //        let user = RavenQuery.Load<Domain.UserProfile.UserProfile>("UserProfile/" + ad.OwnerId.Value)
        //        select new ClassifiedAdDetails()
        //        {
        //            ClassifiedAdId = ad.Id.Value,
        //            Title = ad.Title.Value,
        //            Description = ad.Text.Value,
        //            Price = ad.Price.Amount,
        //            CurrencyCode = ad.Price.Currency.CurrencyCode,
        //            SellersDisplayName = user.DisplayName.Value,
        //        }
        //    ).SingleAsync();

        public static async Task<ClassifiedAdDetails> Query(this IAsyncDocumentSession session, QueryModels.GetPublicClassifiedAd query)
        {
            // Выполняем обычный запрос на получение объявления
            var ad = await session.LoadAsync<Domain.ClassifiedAd.ClassifiedAd>($"ClassifiedAd/{query.ClassifiedAdId}");

            // Если объявление не найдено, возвращаем null или обрабатываем ошибку
            if (ad == null)
            {
                return null;
            }

            // Загружаем профиль пользователя
            var user = await session.LoadAsync<Domain.UserProfile.UserProfile>("UserProfile/" + ad.OwnerId.Value);

            // Возвращаем результат
            return new ClassifiedAdDetails
            {
                ClassifiedAdId = ad.Id.Value,
                Title = ad.Title.Value,
                Description = ad.Text.Value,
                Price = ad.Price.Amount,
                CurrencyCode = ad.Price.Currency.CurrencyCode,
                SellersDisplayName = user?.DisplayName?.Value ?? "Unknown"
            };
        }

        public static Task<List<T>> PagedList<T>(this IRavenQueryable<T> query, int page, int pageSize) =>
            query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public static Task<IEnumerable<PublicClassifiedAdListItem>> Query(
            this DbConnection connection,
            QueryModels.GetPublishedClassifiedAds query)
            => connection.QueryAsync<PublicClassifiedAdListItem>(
                "SELECT \"ClassifiedAdId\", \"Price_Amount\" price, \"Title_Value\" title " +
                "FROM \"ClassifiedAds\" WHERE \"State\"=@State LIMIT @PageSize OFFSET @Offset",
                new
                {
                    State = (int)ClassifiedAdState.Active,
                    PageSize = query.PageSize,
                    Offset = Offset(query.Page, query.PageSize)
                });

        public static Task<IEnumerable<PublicClassifiedAdListItem>> Query(
            this DbConnection connection,
            QueryModels.GetOwnersClassifiedAd query)
            => connection.QueryAsync<PublicClassifiedAdListItem>(
                "SELECT \"ClassifiedAdId\", \"Price_Amount\" price, \"Title_Value\" title " +
                "FROM \"ClassifiedAds\" WHERE \"OwnerId_Value\"=@OwnerId LIMIT @PageSize OFFSET @Offset",
                new
                {
                    OwnerId = query.OwnerId,
                    PageSize = query.PageSize,
                    Offset = Offset(query.Page, query.PageSize)
                });

        public static Task<ClassifiedAdDetails> Query(
            this DbConnection connection,
            QueryModels.GetPublicClassifiedAd query)
            => connection.QuerySingleOrDefaultAsync<ClassifiedAdDetails>(
                "SELECT \"ClassifiedAdId\", \"Price_Amount\" price, \"Title_Value\" title, " +
                "\"Text_Value\" description, \"DisplayName_Value\" sellersdisplayname " +
                "FROM \"ClassifiedAds\", \"UserProfiles\" " +
                "WHERE \"ClassifiedAdId\" = @Id AND \"OwnerId_Value\"=\"UserProfileId\"",
                new { Id = query.ClassifiedAdId });
        private static int Offset(int page, int pageSize) => page * pageSize;
    }
}
