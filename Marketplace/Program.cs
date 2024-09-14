using EventStore.ClientAPI;
using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Marketplace.Tests;
using Marketplace.UserProfile;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

IDocumentStore store = new DocumentStore
{
    Urls = new[] { "http://localhost:8080" },
    Database = "Marketplace_Chapter8",
    Conventions =
        {
            FindIdentityProperty = m => m.Name == "DbId"
        }
};

store.Initialize();

var purgomalumClient = new PurgomalumClient();
var esConnection = EventStoreConnection.Create(
    builder.Configuration.GetConnectionString("EventStore"),
    ConnectionSettings.Create().KeepReconnecting(),
    AppDomain.CurrentDomain.FriendlyName
);
var esStore = new EsAggregateStore(esConnection);
builder.Services.AddSingleton<IAggregateStore>(esStore);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(c => store.OpenAsyncSession());
builder.Services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();
builder.Services.AddDbContext<MarketPlaceDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Marketplace"), o => o.MigrationsAssembly("Marketplace")));
builder.Services.AddScoped<IClassifiedAdRepository, ClassidfiedAdEFCoreRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileEfRepository>();
builder.Services.AddSingleton<ICurrencyLookUp, FakeCurrencyLookup>();
builder.Services.AddScoped<ClassifiedAdsApplicationService>();
builder.Services.AddScoped(c
    => new UserProfileApplicationService(
        c.GetService<IUserProfileRepository>()
        , c.GetService<IUnitOfWork>()
        , text => purgomalumClient.CheckForProfanity(text).GetAwaiter().GetResult())
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
AppBuilderDatabaseExtensions.EnsureDatabase(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
