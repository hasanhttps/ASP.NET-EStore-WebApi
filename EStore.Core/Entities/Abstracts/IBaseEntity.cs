namespace EStore.Domain.Entities.Abstracts;

public interface IBaseEntity {

    // Columns

    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
}
