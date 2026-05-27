using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IDbService
{
    Task<GetPatientDetailsDto> GetPatientDetails(string pesel);
    Task CreateBedAssignment(string pesel, CreateBedAssignmentDto bedAssignment);
    Task<IEnumerable<GetPatientDetailsDto>> GetPatients(string? search);
}