using Template.Domain.Interfaces.Repositories;

namespace Template.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}