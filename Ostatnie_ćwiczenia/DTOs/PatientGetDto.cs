namespace Ostatnie_ćwiczenia.DTOs;

public class PatientGetDto
{
    public string Pesel { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string Sex { get; set; } = null!;
    public List<AdmissionGetDto> Admissions { get; set; } = new();
    public List<BedAssignmentGetDto> BedAssignments { get; set; } = new();
}