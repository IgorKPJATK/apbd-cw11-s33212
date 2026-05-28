
using Microsoft.EntityFrameworkCore;
using Ostatnie_ćwiczenia.DTOs;
using Ostatnie_ćwiczenia.Models;


namespace Ostatnie_ćwiczenia.Service;

public class PatientsService : IPatientsService
{
    private readonly HospitalContext _context;

    public PatientsService(HospitalContext context)
    {
        _context = context;
    }

    public async Task<List<PatientGetDto>> GetPatientsAsync(string? search)
    {
        var query = _context.Patients
            .Include(p => p.Admissions)
                .ThenInclude(a => a.Ward)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.BedType)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.Room)
                        .ThenInclude(r => r.Ward)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = $"%{search}%";
            query = query.Where(p => 
                EF.Functions.Like(p.FirstName, searchTerm) || 
                EF.Functions.Like(p.LastName, searchTerm));
        }

        var patients = await query.Select(p => new PatientGetDto
        {
            Pesel = p.Pesel,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Age = p.Age,
            Sex = p.Sex ? "Male" : "Female",
            Admissions = p.Admissions.Select(a => new AdmissionGetDto
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate,
                Ward = new WardGetDto
                {
                    Id = a.Ward.Id,
                    Name = a.Ward.Name,
                    Description = a.Ward.Description
                }
            }).ToList(),
            BedAssignments = p.BedAssignments.Select(ba => new BedAssignmentGetDto
            {
                Id = ba.Id,
                From = ba.From,
                To = ba.To,
                Bed = new BedGetDto
                {
                    Id = ba.Bed.Id,
                    BedType = new BedTypeGetDto
                    {
                        Id = ba.Bed.BedType.Id,
                        Name = ba.Bed.BedType.Name,
                        Description = ba.Bed.BedType.Description
                    },
                    Room = new RoomGetDto
                    {
                        Id = ba.Bed.Room.Id,
                        HasTv = ba.Bed.Room.HasTv,
                        Ward = new WardGetDto
                        {
                            Id = ba.Bed.Room.Ward.Id,
                            Name = ba.Bed.Room.Ward.Name,
                            Description = ba.Bed.Room.Ward.Description
                        }
                    }
                }
            }).ToList()
        }).ToListAsync();

        return patients;
    }

    public async Task AssignBedAsync(string pesel, AssignBedRequestDto request)
    {
        var patientExists = await _context.Patients.AnyAsync(p => p.Pesel == pesel);
        if (!patientExists)
        {
            throw new Exception($"Pacjent o numerze PESEL {pesel} nie istnieje w systemie.");
        }

        var requestToDate = request.To ?? DateTime.MaxValue;

        var availableBed = await _context.Beds
            .Include(b => b.Room).ThenInclude(r => r.Ward)
            .Include(b => b.BedType)
            .Where(b => b.Room.Ward.Name == request.Ward && b.BedType.Name == request.BedType)
            .Where(b => !b.BedAssignments.Any(ba =>
                ba.From <= requestToDate && 
                (ba.To ?? DateTime.MaxValue) >= request.From
            ))
            .FirstOrDefaultAsync();

        if (availableBed == null)
        {
            throw new Exception($"Brak dostępnego łóżka typu '{request.BedType}' na oddziale '{request.Ward}' we wskazanym terminie.");
        }

        var newAssignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = availableBed.Id,
            From = request.From,
            To = request.To
        };

        _context.BedAssignments.Add(newAssignment);
        await _context.SaveChangesAsync();
    }
}
