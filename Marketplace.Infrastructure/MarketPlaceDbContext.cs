using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.UserProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marketplace.Infrastructure
{
    public class MarketPlaceDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        public DbSet<ClassifiedAd> ClassifiedAds { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassifiedAdEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PictureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileTypeConfiguration());
        }
    }

    public class ClassifiedAdEntityTypeConfiguration : IEntityTypeConfiguration<ClassifiedAd>
    {
        public void Configure(EntityTypeBuilder<ClassifiedAd> builder)
        {
            builder.HasKey(x => x.ClassifiedAdId);
            builder.OwnsOne(x => x.Id);
            builder.OwnsOne(x => x.Price, p => p.OwnsOne(c => c.Currency));
            builder.OwnsOne(x => x.Text);
            builder.OwnsOne(x => x.Title);
            builder.OwnsOne(x => x.ApprovedBy);
            builder.OwnsOne(x => x.OwnerId);
        }
    }

    public class PictureEntityTypeConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(x => x.PictureId);
            builder.OwnsOne(x => x.Id);
            builder.OwnsOne(x => x.ParentId);
            builder.OwnsOne(x => x.Size);
        }
    }

    public class UserProfileTypeConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(x => x.UserProfileId);
            builder.OwnsOne(x => x.Id);
            builder.OwnsOne(x => x.DisplayName);
            builder.OwnsOne(x => x.FullName);
        }
    }

    public static class AppBuilderDatabaseExtensions
    {
        // Статический метод для применения миграций
        public static void EnsureDatabase(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MarketPlaceDbContext>();

                if (!context.Database.EnsureCreated())
                    context.Database.Migrate();
            }
        }
    }
}
