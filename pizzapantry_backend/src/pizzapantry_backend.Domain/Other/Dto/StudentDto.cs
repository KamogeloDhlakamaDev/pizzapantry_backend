namespace pizzapantry_backend.Domain.Other.Dto;

// TODO file not required, can be deleted for development
public sealed class StudentDto
{
    public string FullName { get; set; } = null!;
    
    public int Age { get; set; }
    
    public int Grade { get; set; }

    public string StudentNumber { get; set; } = null!;
}