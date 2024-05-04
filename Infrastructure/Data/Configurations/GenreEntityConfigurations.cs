using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class GenreEntityConfigurations : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(a => a.Id);
        builder .Property(a=>a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Name)
            .HasColumnType("nvarchar(150)")
            .IsRequired(true);
    }
}
