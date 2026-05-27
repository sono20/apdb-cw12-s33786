namespace WebApplication1.DTOs;

public class GetRoomDetailsDto
{
    public string Id { get; set; } = string.Empty;
    public bool HasTv { get; set; }
    public GetWardDetailsDto Ward { get; set; } = null!;
}