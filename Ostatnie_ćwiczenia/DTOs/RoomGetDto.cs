namespace Ostatnie_ćwiczenia.DTOs;

public class RoomGetDto
{
    public string Id { get; set; } = null!;
    public bool HasTv { get; set; }
    public WardGetDto Ward { get; set; } = null!;
}