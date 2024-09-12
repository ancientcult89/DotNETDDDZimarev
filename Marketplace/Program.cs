using Marketplace.Api;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Marketplace.Tests;
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(c => store.OpenAsyncSession());
builder.Services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();
builder.Services.AddDbContext<ClassifiedAdDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Marketplace"), o => o.MigrationsAssembly("Marketplace")));
builder.Services.AddScoped<IClassifiedAdRepository, ClassidfiedAdEFCoreRepository>();
builder.Services.AddSingleton<ICurrencyLookUp, FakeCurrencyLookup>();
builder.Services.AddScoped<ClassifiedAdsApplicationService>();

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
