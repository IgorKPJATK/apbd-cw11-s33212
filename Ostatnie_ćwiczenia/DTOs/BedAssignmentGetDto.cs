namespace Ostatnie_ćwiczenia.DTOs;

public class BedAssignmentGetDto
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
    public BedGetDto Bed { get; set; } = null!;
}