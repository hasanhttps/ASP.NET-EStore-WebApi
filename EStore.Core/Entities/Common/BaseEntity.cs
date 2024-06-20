using EStore.Domain.Entities.Abstracts;

namespace EStore.Domain.Entities.Common;

public abstract class BaseEntity : IBaseEntity {

    // Columns

    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastModifiedAt { get; set; } = DateTime.Now;
}
