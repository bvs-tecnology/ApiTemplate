using System.Diagnostics.CodeAnalysis;
using Infra.Data.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using PushSubscription = Domain.Entities.PushSubscription;

namespace Infra.Data.Context
{
    [ExcludeFromCodeCoverage]
    public class CustomDbContext(DbContextOptions<CustomDbContext> options) : DbContext(options)
    {
        public virtual DbSet<PushSubscription> PushSubscriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("template_api");
            modelBuilder.ApplyConfiguration(new PushSubscriptionConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
