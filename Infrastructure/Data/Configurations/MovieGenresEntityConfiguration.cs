using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MovieGenresEntityConfiguration : IEntityTypeConfiguration<MovieGenres>
{
    public void Configure(EntityTypeBuilder<MovieGenres> builder)
    {
        builder.HasKey(a => new { a.MovieId, a.GenreId });

        builder.Property(a => a.MovieId)
            .ValueGeneratedNever()
            .IsRequired(true);

        builder.Property(a => a.GenreId)
            .ValueGeneratedNever()
            .IsRequired(true);

        builder.HasOne(a => a.Genre)
            .WithMany(a => a.MovieGenres)
            .HasForeignKey(a => a.GenreId);

        builder.HasOne(a => a.Movie)
            .WithMany(a => a.MovieGenres)
            .HasForeignKey(a => a.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
