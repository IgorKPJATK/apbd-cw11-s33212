namespace Ostatnie_ćwiczenia.DTOs;

public class BedGetDto
{
    public int Id { get; set; }
    public BedTypeGetDto BedType { get; set; } = null!;
    public RoomGetDto Room { get; set; } = null!;
}