using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MovieActorsEntityConfiguration : IEntityTypeConfiguration<MovieActors>
{
    public void Configure(EntityTypeBuilder<MovieActors> builder)
    {
        builder.HasKey(a=> new {a.MovieId,a.ActorId});

        builder.Property(a => a.MovieId)
            .ValueGeneratedNever()
            .IsRequired(true);

        builder.Property(a => a.ActorId)
            .ValueGeneratedNever()
            .IsRequired(true);

        builder.Property(a => a.Order)
            .IsRequired(true);

        builder.Property(a => a.Character)
            .HasColumnType("nvarchar(75)")
            .IsRequired(true);

        builder.HasOne(a => a.Movie)
            .WithMany(a => a.MovieActors)
            .HasForeignKey(a=>a.MovieId);

        builder.HasOne(a => a.Actor)
            .WithMany(a => a.MovieActors)
            .HasForeignKey(a => a.ActorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
