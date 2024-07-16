using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RatingEntityConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(a => a.UserId);

        builder.Property(a => a.Rate)
            .HasDefaultValue(0)
            .IsRequired(true)
            .HasColumnType("tinyint");

        builder.ToTable(x => x.HasCheckConstraint("CK_Rate_Range", "[Rate] >= 0 AND [Rate] <= 5"));

        builder.HasOne(a => a.Movie)
            .WithMany(a => a.Ratings)
            .HasForeignKey(a=>a.MovieId);

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a=>a.UserId);
    }
}
