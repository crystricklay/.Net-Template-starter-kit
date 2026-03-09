using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
    public DbSet<User> Users => Set<User>();
}