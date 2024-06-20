using EStore.Domain.Entities.Common;

namespace EStore.Domain.Entities.Concretes;

public class Role : BaseEntity {

    // Columns

    public string RoleName { get; set; }

    // Navigation Property

    public virtual ICollection<User> Users { get; set; }
}
