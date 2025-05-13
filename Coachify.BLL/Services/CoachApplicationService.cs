using AutoMapper;
using Coachify.BLL.DTOs.CoachApplication;
using Coachify.BLL.Interfaces;
using Coachify.DAL;
using Coachify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coachify.BLL.Services;

public class CoachApplicationService : ICoachApplicationService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CoachApplicationService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CoachApplicationDto>> GetAllAsync() =>
        _mapper.Map<IEnumerable<CoachApplicationDto>>(await _db.CoachApplications.ToListAsync());

    public async Task<CoachApplicationDto?> GetByIdAsync(int id)
    {
        var e = await _db.CoachApplications.FindAsync(id);
        return e == null ? null : _mapper.Map<CoachApplicationDto>(e);
    }

    public async Task<IEnumerable<CoachApplicationDto>> GetPendingApplicationsAsync()
    {
        var pending = await _db.CoachApplications
            .Where(a => a.StatusId == 1)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CoachApplicationDto>>(pending);
    }

    public async Task ApproveCoachApplicationAsync(int applicationId)
    {
        var application = await _db.CoachApplications
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
        
        if (application == null)
            throw new InvalidOperationException("Application not found");
        
        if (application.StatusId != 1) 
        {
            throw new InvalidOperationException("Application is not in pending state.");
        }
        
        application.StatusId = 2; 
        
        var coach = new Coach
        {
            CoachId = application.UserId,
            Bio = application.Bio,
            Specialization = application.Specialization,
            Verified = true
        };

        _db.Coaches.Add(coach); 
        await _db.SaveChangesAsync();
    }
    
    public async Task RejectCoachApplicationAsync(int applicationId)
    {
        var application = await _db.CoachApplications.FindAsync(applicationId);

        if (application == null)
            throw new InvalidOperationException("Application not found.");

        if (application.StatusId != 1) 
            throw new InvalidOperationException("Only pending applications can be rejected.");

        application.StatusId = 3; 
        await _db.SaveChangesAsync();
    }



    public async Task<CoachApplicationDto> CreateAsync(CreateCoachApplicationDto dto)
    {
        var e = _mapper.Map<CoachApplication>(dto);
        e.StatusId = 1;
        e.SubmittedAt = DateTime.Now;

        _db.CoachApplications.Add(e);
        await _db.SaveChangesAsync();
        return _mapper.Map<CoachApplicationDto>(e);
    }

    public async Task UpdateAsync(int id, UpdateCoachApplicationDto dto)
    {
        var e = await _db.CoachApplications.FindAsync(id);
        if (e == null) return;

        if (dto.Bio != null)
            e.Bio = dto.Bio;

        bool wasPending = e.StatusId == 1;
        if (dto.StatusId.HasValue)
            e.StatusId = dto.StatusId.Value;

        if (wasPending && dto.StatusId == 2)
        {
            var coach = new Coach
            {
                CoachId = e.UserId,
                User = e.Applicant,
                Bio = e.Bio,
                Specialization = e.Specialization,
                Verified = true
            };
            _db.Coaches.Add(coach);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _db.CoachApplications.FindAsync(id);
        if (e == null) return false;
        _db.CoachApplications.Remove(e);
        await _db.SaveChangesAsync();
        return true;
    }
}