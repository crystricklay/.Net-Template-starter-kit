using Template.Domain.Entities;
using Template.Domain.Interfaces.Repositories;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Template.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}