using Domain.Entities;

namespace Infrastructure.Repositories.Contracts;

public interface IActorRepository
{
    Task CreateActorAsync(Actor actor,CancellationToken cancellationToken);
    Task UpdateActorAsync(Actor actor,CancellationToken cancellationToken);
    Task DeleteActorAsync(Actor actor,CancellationToken cancellationToken);
    Task<Actor?> GetActorByIdAsync(int actorId,CancellationToken cancellationToken);
    Task<Actor?> GetActorByIdAsNoTrackingAsync(int actorId, CancellationToken cancellationToken);
}