using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a=>a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Title)
            .HasColumnType("nvarchar(100)")
            .IsRequired(true);

        builder.Property(a => a.Summery)
            .HasColumnType("nvarchar(1000)")
            .IsRequired(true);

        builder.Property(a => a.Trailer)
            .HasColumnType("nvarchar(750)")
            .IsRequired(false);

        builder.Property(a => a.InTheaters)
            .IsRequired(true)
            .HasDefaultValue(false);

        builder.Property(a => a.ReleaseDate)
            .IsRequired(false);

        builder.Property(a => a.Poster)
            .HasColumnType("nvarchar(750)")
            .IsRequired(true);
    }
}
