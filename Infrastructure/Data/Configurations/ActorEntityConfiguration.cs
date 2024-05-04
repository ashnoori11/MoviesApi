using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ActorEntityConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Name)
            .HasColumnType("nvarchar(100)")
            .IsRequired(true);

        builder.Property(a => a.Biography)
            .HasColumnType("nvarchar(850)")
            .IsRequired(false);

        builder.Property(a => a.Picture)
            .HasColumnType("nvarchar(200)")
            .IsRequired(false);

        builder.Property(a => a.RowVersion)
            .IsRowVersion();
    }
}
