using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MovieTheaterMoviesEntityConfiguration : IEntityTypeConfiguration<MovieTheaterMovies>
{
    public void Configure(EntityTypeBuilder<MovieTheaterMovies> builder)
    {
        builder.HasKey(a=> new { a.MovieId,a.MovieTheaterId});

        builder.Property(a => a.MovieId)
            .ValueGeneratedNever()
            .IsRequired(true);

        builder.Property(a => a.MovieTheaterId)
            .ValueGeneratedNever()
            .IsRequired(true);

        builder.HasOne(a => a.MovieTheater)
            .WithMany(a => a.MovieTheaterMovies)
            .HasForeignKey(a=>a.MovieTheaterId);

        builder.HasOne(a => a.Movie)
            .WithMany(a => a.MovieTheaterMovies)
            .HasForeignKey(a => a.MovieId);
    }
}
