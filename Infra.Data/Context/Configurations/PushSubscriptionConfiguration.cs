using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Context.Configurations;

public class PushSubscriptionConfiguration : IEntityTypeConfiguration<PushSubscription>
{
    public void Configure(EntityTypeBuilder<PushSubscription> builder)
    {
        builder.ToTable("push_subscriptions");

        #region Audit
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid").IsRequired().ValueGeneratedNever();
        builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasColumnType("uuid");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz").IsRequired();
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamptz");
        builder.HasIndex(e => e.CreatedBy);
        #endregion

        #region Entity Properties
        builder.Property(e => e.Auth).HasColumnName("auth").HasColumnType("text").IsRequired();
        builder.Property(e => e.Endpoint).HasColumnName("endpoint").HasColumnType("text").IsRequired();
        builder.HasIndex(e => e.Endpoint).IsUnique();
        builder.Property(e => e.P256dh).HasColumnName("p256dh").HasColumnType("text").IsRequired();

        #endregion
    }
}