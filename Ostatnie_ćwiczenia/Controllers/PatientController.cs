using Microsoft.AspNetCore.Mvc;
using Ostatnie_ćwiczenia.DTOs;
using Ostatnie_ćwiczenia.Service;

namespace ostatnie_ćwiczenia.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _patientsService;

    public PatientsController(IPatientsService patientsService)
    {
        _patientsService = patientsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] string? search)
    {
        var patients = await _patientsService.GetPatientsAsync(search);
        return Ok(patients);
    }

    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AssignBed(string pesel, [FromBody] AssignBedRequestDto request)
    {
        try
        {
            await _patientsService.AssignBedAsync(pesel, request);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}