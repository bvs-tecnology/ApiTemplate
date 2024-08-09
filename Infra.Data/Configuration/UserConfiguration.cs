using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "AF");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").HasDefaultValueSql("newid()").IsRequired();
        builder.Property(e => e.Login).HasColumnName("Login").HasColumnType("varchar").HasMaxLength(20).IsRequired();
        builder.Property(e => e.Hash).HasColumnName("Hash").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(e => e.Salt).HasColumnName("Salt").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        builder.Property(e => e.PersonId).HasColumnName("IdPerson").HasColumnType("uniqueidentifier").IsRequired();
        builder.Property(e => e.Role).HasColumnName("Role").HasColumnType("int").IsRequired();

        builder.HasOne(e => e.Person).WithMany(x => x.Users).HasForeignKey(e => e.PersonId);
    }
}