namespace pizzapantry_backend.Infrastructure.Persistence.Seeds;

// TODO remove the file for development 
public static class StudentSeed
{
    public static IEnumerable<Student> CreateStudents()
    {
        return new List<Student>
        {
            new()
            {
                strFirstName = "Elias",
                strLastName = "Railis",
                strCreatedBy = "from-seed",
                iAge = 18,
                iGrade = 10,
                bIsActive = true,
                bIsDeleted = false,
                strStudentNumber = "6783227D-4FA2-4E74-90B5-9A7D8C75CC33"
            },
            new()
            {
                strFirstName = "Joe",
                strLastName = "Doe",
                strCreatedBy = "from-seed",
                iAge = 16,
                iGrade = 7,
                bIsActive = true,
                bIsDeleted = false,
                strStudentNumber = "70D9DFB5-F619-4C71-934D-CFC02A78211A"
            }
        };
    }
}