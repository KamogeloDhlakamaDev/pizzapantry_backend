namespace pizzapantry_backend.Domain.Common;

public class AuditableEntity
{
    [Column(Order = 1)]
    public DateTime dtCreated { get; set; }

    [Column(Order = 2)]
    public string strCreatedBy { get; set; } = null!;
    
    [Column(Order = 3)]
    public DateTime? dtEdited { get; set; }
    
    [Column(Order = 4)]
    public string? strEditedBy { get; set; }
}