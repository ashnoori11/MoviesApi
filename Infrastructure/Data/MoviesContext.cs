using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infrastructure.Data;

public class MoviesContext : IdentityDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("moviesDb"),
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            .EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), Array.Empty<int>())
            .UseNetTopologySuite());
    }

    #region entities
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<MovieTheater> MovieTheaters { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieGenres> MovieGenres { get; set; }
    public DbSet<MovieTheaterMovies> MovieTheaterMovies { get; set; }
    public DbSet<MovieActors> MovieActors { get; set; }
    #endregion 
}
