namespace pizzapantry_backend.Domain.Entities;

// TODO file not required, can be deleted for development
public sealed class Student : AuditableEntity
{
    [Column(Order = 0)]
    public int iStudentId { get; set; }

    public string strFirstName { get; set; } = null!;

    public string strLastName { get; set; } = null!;
    
    public int iAge { get; set; }
    
    public int iGrade { get; set; }

    public string strStudentNumber { get; set; } = null!;
    
    public bool bIsActive { get; set; }
    
    public bool bIsDeleted { get; set; }
}