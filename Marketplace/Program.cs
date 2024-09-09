using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Marketplace.Tests;
using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

IDocumentStore store = new DocumentStore
{
    Urls = new[] { "http://localhost:8080" },
    Database = "Marketplace_Chapter8",
    Conventions =
        {
            FindIdentityProperty = m => m.Name == "_databaseId"
        }
};

store.Initialize();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(c => store.OpenAsyncSession());
builder.Services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
builder.Services.AddSingleton<ICurrencyLookUp, FakeCurrencyLookup>();
builder.Services.AddScoped<ClassifiedAdsApplicationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
