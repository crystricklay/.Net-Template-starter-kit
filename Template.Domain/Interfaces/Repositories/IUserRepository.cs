using Template.Domain.Entities;
using Template.Domain.Interfaces.Repositories.Base;

namespace Template.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}