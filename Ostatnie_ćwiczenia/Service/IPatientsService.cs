using Ostatnie_ćwiczenia.DTOs;

namespace Ostatnie_ćwiczenia.Service;

public interface IPatientsService
{
    Task<List<PatientGetDto>> GetPatientsAsync(string? search);
    Task AssignBedAsync(string pesel, AssignBedRequestDto request);
}