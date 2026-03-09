using Template.Domain.Entities.Base;
using Template.Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Interfaces.Repositories;
using Template.Infrastructure.Persistence;

namespace Template.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IUserRepository Users { get; }
  

    public UnitOfWork(
        ApplicationDbContext context,
        IUserRepository userRepository
        )
    {
        _context = context;
        Users = userRepository;
        
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = _context.ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await _context.SaveChangesAsync(cancellationToken);
    }
}