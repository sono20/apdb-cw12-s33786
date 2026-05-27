namespace WebApplication1.DTOs;

public class GetPatientDetailsDto
{
    public string Pesel { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Sex { get; set; } = string.Empty;
    public IEnumerable<GetAdmissionDetailsDto> Admissions { get; set; } = [];
    public IEnumerable<GetBedAssignmentDetailsDto> BedAssignments { get; set; } = [];
    
}