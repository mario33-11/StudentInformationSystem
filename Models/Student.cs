using System.ComponentModel.DataAnnotations;

namespace StudentInformationSystem.Models;

public class Student
{
    [Key]
    public int StudentID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }

    [Required]
    public string Department { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow.Date;
}


