namespace Ostatnie_ćwiczenia.DTOs;

public class AdmissionGetDto
{
    public int Id { get; set; }
    public DateTime AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public WardGetDto Ward { get; set; } = null!;
}