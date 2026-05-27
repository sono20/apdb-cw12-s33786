namespace WebApplication1.DTOs;

public class GetBedAssignmentDetailsDto
{
    public int Id { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public GetBedDetailsDto Bed { get; set; } = null!;

}