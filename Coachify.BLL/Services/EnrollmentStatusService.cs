using AutoMapper;
using Coachify.BLL.DTOs.Enrollment;
using Coachify.BLL.Interfaces;
using Coachify.DAL;

namespace Coachify.BLL.Services;

public class EnrollmentStatusService : IEnrollmentStatusService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public EnrollmentStatusService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EnrollmentStatusDto>> GetAllAsync()
    {
        var statuses = await Task.FromResult(_db.EnrollmentStatuses.ToList());
        return _mapper.Map<IEnumerable<EnrollmentStatusDto>>(statuses);
    }
}