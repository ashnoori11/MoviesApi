using Infrastructure.Common.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.UnitOfWorks;

public class UnitOfwork : IDisposable, IUnitOfWork
{
    public MoviesContext _context { get; }

    public UnitOfwork(MoviesContext context)
    {
        _context = context;
    }

    #region query repository
    public IQueryRepository<T> GetQueryRepository<T>() where T : class
        => new QueryRepository<T>(_context);
    #endregion

    #region genre repository
    private IGenresRepository _genreRepository;
    public IGenresRepository GenreRepository
    {
        get
        {
            if (_genreRepository is null)
                _genreRepository = new GenresRepository(_context);

            return _genreRepository;
        }
    }
    #endregion

    #region actor repository
    private IActorRepository _actorRepository;
    public IActorRepository ActorRepository
    {
        get
        {
            if (_actorRepository is null)
                _actorRepository = new ActorRepository(_context);

            return _actorRepository;
        }
    }
    #endregion

    #region MovieTheater Repository
    private IMovieTheaterRepository _movieTheaterRepository;
    public IMovieTheaterRepository MovieTheaterRepository 
    {
        get
        {
            if (_movieTheaterRepository is null)
                _movieTheaterRepository = new MovieTheaterRepository(_context);

            return _movieTheaterRepository;
        }
    }
    #endregion

    #region MovieRepository
    private IMovieRepository _movieRepository;
    public IMovieRepository MovieRepository
    {
        get
        {
            if (_movieRepository is null)
                _movieRepository = new MovieRepository(_context);

            return _movieRepository;
        }
    }
    #endregion

    public async Task<SaveChangeStatus> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return SaveChangeStatus.Succeed;
        }
        catch (Exception ex)
        {
            if (ex is DbUpdateConcurrencyException)
                return SaveChangeStatus.Concurrent;
            else
                return SaveChangeStatus.Failure;
        }
    }
    public async Task SaveChangesAsync<T>(CancellationToken cancellationToken)
    {
        var saved = false;
        while (!saved)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                saved = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is T)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        if (databaseValues is null)
                            continue;

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database

                            //< value to be saved >;
                            proposedValues[property] = databaseValue; 
                        }

                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
            }
        }
    }

    public void Dispose() => _context.Dispose();
}
