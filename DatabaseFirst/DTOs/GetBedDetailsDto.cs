namespace WebApplication1.DTOs;

public class GetBedDetailsDto
{
    public int Id { get; set; }
    public GetBedTypeDetailsDto BedType { get; set; } = null!;
    public GetRoomDetailsDto Room { get; set; } = null!;
}