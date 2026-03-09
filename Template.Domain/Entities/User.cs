using Template.Domain.Entities.Base;

namespace Template.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = default!;
}