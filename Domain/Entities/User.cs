using Domain.Entities.Enums;
using static System.String;

namespace Domain.Entities;

public class User : BaseEntity
{
    public User() { }
    public string Login { get; private set; } = Empty;
    public string Hash { get; private set; } = Empty;
    public string Salt { get; private set; } = Empty;
    public Guid PersonId { get; set; }
    public UserRole Role { get; private set; }

    #region Mapping
    public virtual Person? Person { get; set; }
    #endregion
}