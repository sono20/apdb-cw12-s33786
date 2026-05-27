namespace WebApplication1.DTOs;

public class GetAdmissionDetailsDto
{
    public int Id { get; set; }
    public DateTime? AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public GetWardDetailsDto Ward { get; set; } = null!;
}