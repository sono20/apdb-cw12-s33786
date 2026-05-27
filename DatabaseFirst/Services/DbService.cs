using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services;

public class DbService : IDbService
{
    private readonly ApbdContext _dbContext;

    public DbService(ApbdContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetPatientDetailsDto> GetPatientDetails(string pesel)
    {
        var res = await _dbContext.Patients
            .Where(e => e.Pesel == pesel)
            .Select(e => new GetPatientDetailsDto()
            {
                Pesel = e.Pesel,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Admissions = e.Admissions.Select(a => new GetAdmissionDetailsDto()
                {
                    Id = a.Id,
                    AdmissionDate = a.AdmissionDate,
                    DischargeDate = a.DischargeDate,
                    Ward = new GetWardDetailsDto
                    {
                        Id = a.Ward.Id,
                        Name = a.Ward.Name
                    }
                }),
                BedAssignments = e.BedAssignments.Select(ba => new GetBedAssignmentDetailsDto()
                {
                    Id = ba.Id,
                    From = ba.From,
                    To = ba.To,
                    Bed = new GetBedDetailsDto
                    {
                        Id = ba.BedId,
                        BedType = new GetBedTypeDetailsDto
                        {
                            Id = ba.Bed.BedTypeId,
                            Name = ba.Bed.BedType.Name
                        },
                        Room = new GetRoomDetailsDto
                        {
                            Id = ba.Bed.RoomId,
                            Ward = new GetWardDetailsDto
                            {
                                Id = ba.Bed.Room.WardId,
                                Name = ba.Bed.Room.Ward.Name
                            }
                        }
                    }
                })
            }).FirstOrDefaultAsync();

        if (res == null)
        {
            throw new NotFoundException();
        }
        
        return res;
    }

    public async Task CreateBedAssignment(string pesel, CreateBedAssignmentDto bedAssignment)
    {
        var anyPatient = await _dbContext.Patients.AnyAsync(e => e.Pesel == pesel);
        if (!anyPatient)
        {
            throw new NotFoundException($"Pacjent o numerze PESEL {pesel} nie istnieje");
        }

        var availableBed = await _dbContext.Beds
            .Where(b => b.BedType.Name == bedAssignment.BedType)
            .Where(b => b.Room.Ward.Name == bedAssignment.Ward)
            .Where(b => !b.BedAssignments.Any(ba =>
                bedAssignment.From < ba.To && bedAssignment.To > ba.From
            )).FirstOrDefaultAsync();

        if (availableBed == null)
        {
            throw new NotFoundException(
                $"Brak wolnych łóżek typu '{bedAssignment.BedType}' na oddziale '{bedAssignment.Ward}' w wybranym terminie");
        }

        var newAssignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = availableBed.Id,
            From = bedAssignment.From,
            To = bedAssignment.To
        };
        
        await _dbContext.BedAssignments.AddAsync(newAssignment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetPatientDetailsDto>> GetPatients(string? search)
    {
        var query = _dbContext.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string SearchPattern = $"%{search}%";
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName, SearchPattern) ||
                EF.Functions.Like(p.LastName, SearchPattern));
        }

        var result = await query.Select(p => new GetPatientDetailsDto
        {
            Pesel = p.Pesel,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Admissions = p.Admissions.Select(a => new GetAdmissionDetailsDto
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate,
                Ward = new GetWardDetailsDto
                {
                    Id = a.Ward.Id,
                    Name = a.Ward.Name
                }
            }),
            BedAssignments = p.BedAssignments.Select(ba => new GetBedAssignmentDetailsDto
            {
                Id = ba.Id,
                From = ba.From,
                To = ba.To,
                Bed = new GetBedDetailsDto
                {
                    Id = ba.BedId,
                    BedType = new GetBedTypeDetailsDto
                    {
                        Id = ba.Bed.BedTypeId,
                        Name = ba.Bed.BedType.Name
                    },
                    Room = new GetRoomDetailsDto
                    {
                        Id = ba.Bed.RoomId,
                        Ward = new GetWardDetailsDto
                        {
                            Id = ba.Bed.Room.WardId,
                            Name = ba.Bed.Room.Ward.Name
                        }
                    }
                }
            })
        }).ToListAsync();

        return result;
    }
}