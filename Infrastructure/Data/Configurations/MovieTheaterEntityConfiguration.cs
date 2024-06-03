using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MovieTheaterEntityConfiguration : IEntityTypeConfiguration<MovieTheater>
{
    public void Configure(EntityTypeBuilder<MovieTheater> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a=>a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Name)
            .HasColumnType("nvarchar(75)")
            .IsRequired(true);


    }
}
