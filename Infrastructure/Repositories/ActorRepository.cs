using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ActorRepository : IActorRepository
{
    #region constructor
    private readonly MoviesContext _context;
    public ActorRepository(MoviesContext moviesContext)
    {
        _context = moviesContext;
    }
    #endregion

    public async Task CreateActorAsync(Actor actor, CancellationToken cancellationToken)
        => await _context.AddAsync(actor, cancellationToken);

    public async Task DeleteActorAsync(Actor actor, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _context.Remove(actor);
        await Task.CompletedTask;
    }

    public async Task<Actor?> GetActorByIdAsync(int actorId, CancellationToken cancellationToken)
        => await _context.Actors.FirstOrDefaultAsync(a => a.Id == actorId, cancellationToken);

    public async Task<List<Actor>> GetActorsByIsdNoTrackingAsync(int[] actorIds, CancellationToken cancellationToken)
    => await _context.Actors
        .AsNoTracking()
        .Where(a => actorIds.Contains(a.Id))
        .ToListAsync(cancellationToken);


    public async Task UpdateActorAsync(Actor actor, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _context.Update(actor);
        await Task.CompletedTask;
    }

    public async Task<Actor?> GetActorByIdAsNoTrackingAsync(int actorId, CancellationToken cancellationToken)
        => await _context.Actors.FirstOrDefaultAsync(a => a.Id == actorId, cancellationToken);
}
