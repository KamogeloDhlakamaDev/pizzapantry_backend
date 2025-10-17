using pizzapantry_backend.Domain.Other.Dto;

namespace pizzapantry_backend.Domain.Contracts.Responses;

// TODO file not required, can be deleted for development
public class AllStudentsResponse
{
    public List<StudentDto> Students { get; set; } = new();
}