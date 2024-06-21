namespace EStore.Domain.Helpers;

public class RefreshToken {

    // Fields

    public string Token { get; set; }
    public DateTime ExpireTime { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
}
