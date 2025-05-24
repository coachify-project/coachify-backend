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
        var application = await _db.CoachApplications.FindAsync(id);
        return application == null ? null : _mapper.Map<CoachApplicationDto>(application);
    }

    public async Task<IEnumerable<CoachApplicationDto>> GetPendingApplicationsAsync()
    {
        var pending = await _db.CoachApplications
            .Where(a => a.StatusId == 1) // 1 = Pending
            .ToListAsync();

        return _mapper.Map<IEnumerable<CoachApplicationDto>>(pending);
    }

    public async Task ApproveCoachApplicationAsync(int applicationId)
    {
        var application = await _db.CoachApplications
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null)
            throw new InvalidOperationException("Application not found.");

        if (application.StatusId != 1)
            throw new InvalidOperationException("Application is not in pending state.");

        application.StatusId = 2; // Approved
        
        var coach = new Coach
        {
            CoachId = application.UserId,
            Bio = application.Bio,
            Specialization = application.Specialization,
            Verified = true
        };
        _db.Coaches.Add(coach);

        if (application.Applicant != null)
        {
            application.Applicant.RoleId = 3; // Coach
        }

        await _db.SaveChangesAsync();
    }

    public async Task RejectCoachApplicationAsync(int applicationId)
    {
        var application = await _db.CoachApplications
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null)
            throw new InvalidOperationException("Application not found.");

        if (application.StatusId != 1)
            throw new InvalidOperationException("Only pending applications can be rejected.");

        application.StatusId = 3; // Rejected

        if (application.Applicant != null && application.Applicant.RoleId != 3)
        {
            application.Applicant.RoleId = 2;// Client
        }

        await _db.SaveChangesAsync();
    }

    public async Task<CoachApplicationDto> CreateAsync(CreateCoachApplicationDto dto)
    {
        var application = _mapper.Map<CoachApplication>(dto);

        application.StatusId = 1; //Pending
        application.SubmittedAt = DateTime.UtcNow;
        
        _db.CoachApplications.Add(application);
        await _db.SaveChangesAsync();

        return _mapper.Map<CoachApplicationDto>(application);
    }
    

    public async Task<bool> DeleteAsync(int id)
    {
        var application = await _db.CoachApplications.FindAsync(id);
        if (application == null) return false;

        _db.CoachApplications.Remove(application);
        await _db.SaveChangesAsync();
        return true;
    }
}
